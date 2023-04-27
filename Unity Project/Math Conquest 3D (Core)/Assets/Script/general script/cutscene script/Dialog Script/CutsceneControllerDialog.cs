using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class CutsceneControllerDialog : MonoBehaviour
{
    [Header("Index of Dialogs (private)")]
    private int dialogIndex = 0;
    private bool isDialogActive;
    private bool isWaitingAfterDialogEnd;
    private bool canForceEndDialog = false;
    private int dialogSetIndex = 0;

    [Header("Game Controller Object")]
    public ExplorationModeGameController GameController;
    public DialogManager DialogManager;

    private string speakerPreviousSpriteName;
    private string backgroundImagePreviousName = "no background";
    private Animator speakerImageAnimator;
    private GameObject speakerEmotionEffectObject;

    [Header("Dialog Typing and Waiting Time")]
    private float typeLetterInterval = 0.035f;
    private float nextDialogWaitTime = 0.2f;
    private float backgroundTransitionMaxtWaitTime = 0.3f;
    private float backgroundTransitionCurrentWaitTime = 0f;

    [Header("Dialog Repeat Setting")]
    public bool isDialogRepeatable = false;
    public bool isDialogDisabledAfterFinish = false;
    [SerializeField] private bool isDialogSelfDestroyAfterFinish = false;
    private bool isDialogObjectActivated = false;
    private bool isDialogObjectDisabled = false;
    private bool isDialogPlayed = false;

    [Header("Active End Dialog Object")]
    public List<GameObject> activeAfterEndDialogObjectList = new List<GameObject>();
    public List<GameObject> disableAfterEndDialogObjectList = new List<GameObject>();

    [Header("Dialog Data")]
    public List<DialogScriptableObjectClass> dialogSettList =  new List<DialogScriptableObjectClass>();
    private Queue<string> dialogQueue = new Queue<string>();

    private MasterInput playerInput;

    private void Awake()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        DialogManager.speakerText.GetComponent<TMP_Text>();
        DialogManager.dialogText.GetComponent<TMP_Text>();
        DialogManager.speakerImage.GetComponent<Image>();
        DialogManager.dialogButton.GetComponent<Animator>();

        speakerImageAnimator = DialogManager.speakerImage.GetComponent<Animator>();
    }
    
    public void StartDialogCutscene()
    {
        if (isDialogPlayed == false)
        {
            isDialogPlayed = true;
            DialogManager.DialogUI.SetActive(true);
            SetupDialog();
            PlayNextDialog();
            GameController.TriggerCutscene();
            DialogManager.zetaAnimator.GetComponent<Animator>().SetBool("isZetaHumanForm", true);
            GameObject particleShifting = Instantiate(DialogManager.zetaShapeShiftParticle, DialogManager.zetaAnimator.position + (Vector3.down * 0.2f), DialogManager.zetaAnimator.rotation);
            Destroy(particleShifting, 1f);
        }
    }
    private void EndDialogCutscene()
    {
        DialogManager.dialogText.text = "";
        DialogManager.DialogUI.SetActive(false);
        RemoveControl();
        GameController.AllowMovement();
        DestroySpeakerEmotionEffectSprite();
        CountDialogSet();
        DialogManager.zetaAnimator.GetComponent<Animator>().SetBool("isZetaHumanForm", false);
        GameObject particleShifting = Instantiate(DialogManager.zetaShapeShiftParticle, DialogManager.zetaAnimator.position + (Vector3.down * 0.15f), DialogManager.zetaAnimator.rotation);
        particleShifting.transform.localScale -= new Vector3(.5f,.5f,.5f);
        Destroy(particleShifting, 1f);

        if (isDialogRepeatable == true)
        {
            isDialogPlayed = false;
        }
        if(isDialogSelfDestroyAfterFinish == true)
        {
            Destroy(this.gameObject);
        }
        if (isDialogDisabledAfterFinish == true)
        {
            this.gameObject.SetActive(false);
        }
        if(activeAfterEndDialogObjectList.Count > 0)
        {
            ActiveObjectAtEndDialog();
        }
        if(disableAfterEndDialogObjectList.Count > 0)
        {
            DisableObjectAtEndDialog();
        }
    }
    private void CountDialogSet()
    {
        if(dialogSetIndex + 1 < dialogSettList.Count)
        {
            dialogSetIndex++;
        }
    }

    private void SetupDialog()
    {
        dialogIndex = 0;
        dialogQueue.Clear();
        for (int i = 0; i < dialogSettList[dialogSetIndex].dialogClass.DialogSet.Count; i++)
        {
            dialogQueue.Enqueue(dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].dialogString);
        }
    }
    public void PlayNextDialog()
    {
        if (isWaitingAfterDialogEnd == false)
        {
            StopDialogButtonConfirmAnimation();
            PlayDialogButtonConfirmAnimation();
            CheckDialogUiActivation();
            CheckDialogQueue();
            CheckDialogPlaying();
        }
    }
    private void CheckDialogUiActivation()
    {
        if (DialogManager.DialogUI.activeSelf == false)
        {
            return;
        }
    }
    private void CheckDialogQueue()
    {
        if (dialogQueue.Count == 0)
        {
            EndDialogCutscene();
            return;
        }
    }
    private void CheckDialogPlaying()
    {
        if (isDialogActive == false && canForceEndDialog == false)
        {
            DialogStart();
        }

        else if (isDialogActive == true && canForceEndDialog == true)
        {
            DialogForceEnd();
        }
    }
    private void DialogStart()
    {
        if(dialogIndex > dialogSettList[dialogSetIndex].dialogClass.DialogSet.Count - 1) { return; }
        DialogManager.speakerText.text = dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerString;
        DialogManager.dialogText.text = "";
        SetSpeakerImage();
        SetBackgroundImage();
        PlaySpeakerAnimation();
        PlaySpeakerEmotionEffect();
        PlayDialogButtonTypeAnimation();
        StartCoroutine(TypeDialog(dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].dialogString));
    }
    private void SetSpeakerImage()
    {
        if (CheckIfSpeakerImageIsAvailable())
        {
            DialogManager.speakerImage.gameObject.SetActive(true);
            DialogManager.speakerImage.overrideSprite = dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerSprite;
        }
        else
        {
            DialogManager.speakerImage.gameObject.SetActive(false);
        }
    }
    private void SetBackgroundImage()
    {
        SetBackgroundTransitionWaitTime();

        if (CheckIfBackgroundImageIsAvailable())
        {
            if (CheckIfBackgroundIsNew())
            {
                CreateBackgroundTransition();
                backgroundImagePreviousName = dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].backgroundSprite.name;
            }

            DialogManager.dialogText.text = "";
            DialogManager.backgroundImage.gameObject.SetActive(true);
            DialogManager.backgroundImage.overrideSprite = dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].backgroundSprite;
        }
        else if (!CheckIfBackgroundImageIsAvailable() && backgroundImagePreviousName != "no background")
        {
            CreateBackgroundTransition();
            DialogManager.backgroundImage.gameObject.SetActive(false);
            backgroundImagePreviousName = "no background";
        }
        else if (!CheckIfBackgroundImageIsAvailable() && backgroundImagePreviousName == "no background")
        {
            DialogManager.backgroundImage.gameObject.SetActive(false);
        }
    }
    private void SetBackgroundTransitionWaitTime()
    {
        if (CheckIfBackgroundImageIsAvailable())
        {
            backgroundTransitionCurrentWaitTime = backgroundTransitionMaxtWaitTime;
        }
        else
        {
            backgroundTransitionCurrentWaitTime = 0f;
        }
    }
    private void CreateBackgroundTransition()
    {
        GameObject backgroundTransition = Instantiate(DialogManager.backgroundTransitionPrefab);
        backgroundTransition.transform.SetParent(DialogManager.inGameCanvas.transform); 
        backgroundTransition.transform.SetAsLastSibling();
    }
    private bool CheckIfSpeakerImageIsAvailable()
    {
        return dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerSprite != null;
    }
    private bool CheckIfBackgroundImageIsAvailable()
    {
        return dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].backgroundSprite != null;
    }
 
    private void PlaySpeakerAnimation()
    {
        if (CheckIfSpeakerImageIsAvailable() == false)
        {
            return;
        }
        if (CheckIfSpeakerIsNew())
        {
            speakerImageAnimator.SetTrigger("triggerSpeakerImageEntry");
        }
        speakerPreviousSpriteName = dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerSprite.name;
    }
    private bool CheckIfSpeakerIsNew()
    {
        return dialogIndex > 0 && speakerPreviousSpriteName != dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerSprite.name;
    }
    private bool CheckIfBackgroundIsNew()
    {
        return backgroundImagePreviousName != dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].backgroundSprite.name;
    }
    private void PlaySpeakerEmotionEffect()
    {
        DestroySpeakerEmotionEffectSprite();

        if (!CheckIfSpeakerImageIsAvailable())
        {
            return;
        }
        if (CheckIfSpeakerEmotionEffectIsAvailable())
        {
            speakerEmotionEffectObject = Instantiate(dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerEmotionEffect);
            speakerEmotionEffectObject.transform.SetParent(DialogManager.speakerEffectSpawnpoint);
            speakerEmotionEffectObject.GetComponent<Animator>().SetTrigger(dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerEmotionEffect.name); ;
        }
    }
    private bool CheckIfSpeakerEmotionEffectIsAvailable()
    {
        return dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].speakerEmotionEffect != null ;
    }
    private void DestroySpeakerEmotionEffectSprite()
    {
        if (speakerEmotionEffectObject != null)
        {
            Destroy(speakerEmotionEffectObject);
        }
    }

    private void DialogForceEnd()
    {
        StopAllCoroutines();
        DialogManager.dialogText.text = dialogSettList[dialogSetIndex].dialogClass.DialogSet[dialogIndex].dialogString;
        DialogEnd();
    }
    IEnumerator TypeDialog(string dialog)
    {
        canForceEndDialog = false;
        yield return new WaitForSeconds(backgroundTransitionCurrentWaitTime);

        isDialogActive = true;
        canForceEndDialog = true;
        foreach (char letterOfDialog in dialog.ToCharArray())
        {
            yield return new WaitForSeconds(typeLetterInterval);
            DialogManager.dialogText.text += letterOfDialog;
            yield return null;
        }
        DialogEnd();
    }
    private void DialogEnd()
    {
        canForceEndDialog = false;
        dialogIndex++;

        StopDialogButtonConfirmAnimation();
        StartCoroutine(DialogEndWaiting());

        isDialogActive = false;
        dialogQueue.Dequeue();

        PlayDialogButtonWaitAnimation();
    }
    private void PlayDialogButtonTypeAnimation()
    {
        DialogManager.dialogButton.SetBool("isDialogButtonPlaying", true);
    }
    private void PlayDialogButtonWaitAnimation()
    {
        DialogManager.dialogButton.SetBool("isDialogButtonPlaying", false);
    }
    private void PlayDialogButtonConfirmAnimation()
    {
        if(dialogIndex > 0)
        {
            DialogManager.dialogButton.SetBool("isDialogButtonConfirm", true);
        }
    }
    private void StopDialogButtonConfirmAnimation()
    {
        DialogManager.dialogButton.SetBool("isDialogButtonConfirm", false);
    }

    IEnumerator DialogEndWaiting()
    {
        isWaitingAfterDialogEnd = true;
        yield return new WaitForSeconds(nextDialogWaitTime);
        isWaitingAfterDialogEnd = false;
    }
    private void ActiveObjectAtEndDialog()
    {
        if (isDialogObjectActivated == false)
        {
            isDialogObjectActivated = true;
            LoopActiveObject();
        }
    }
    private void LoopActiveObject()
    {
        foreach(GameObject activeObject in activeAfterEndDialogObjectList)
        {
            if(activeObject != null)
            {
                activeObject.SetActive(true);
                if(activeObject.TryGetComponent<BoxCollider>(out BoxCollider collider))
                {
                    collider.center = new Vector3(0,5,0);
                    collider.center = new Vector3(0,0,0);
                }
            }
        }
    }
    private void DisableObjectAtEndDialog()
    {
        if(isDialogObjectDisabled == false)
        {
            isDialogObjectDisabled = true;
            LoopDisableObject();
        }
    }
    private void LoopDisableObject()
    {
        foreach(GameObject disableObject in disableAfterEndDialogObjectList)
        {
            if(disableObject != null)
                disableObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            SetupControl();
            StartDialogCutscene();
        }
    }
    public void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlGeneral.NextDialog.performed += context => PlayNextDialog();
        playerInput.Enable();
    }
    private void RemoveControl()
    {
        playerInput.PlayerControlGeneral.NextDialog.performed -= context => PlayNextDialog();
        playerInput.Disable();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CutsceneControllerDialog))]
public class CutsceneControllerDialogTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CutsceneControllerDialog dialog = (CutsceneControllerDialog)target;

        EditorGUI.BeginDisabledGroup(dialog.DialogManager.DialogUI.activeSelf == true);
        if (GUILayout.Button("Start Dialog")) {dialog.StartDialogCutscene(); dialog.SetupControl();}
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(dialog.DialogManager.DialogUI.activeSelf == false);
        if (GUILayout.Button("Play Next Dialog")) {dialog.PlayNextDialog();}
        EditorGUI.EndDisabledGroup();
    }
}
#endif