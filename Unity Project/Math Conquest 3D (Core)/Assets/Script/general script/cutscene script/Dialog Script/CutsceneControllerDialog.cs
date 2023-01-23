using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class CutsceneControllerDialog : MonoBehaviour
{
    [Header("Index of Dialogs (private)")]
    [SerializeField] private int dialogIndex = 0;
    [SerializeField] private bool isDialogActive;
    [SerializeField] private bool isWaitingAfterDialogEnd;
    [SerializeField] private bool canForceEndDialog = false;
    [SerializeField] private int dialogSetIndex = 0;

    [Header("Game Controller Object")]
    public ExplorationModeGameController GameController;

    [Header("Dialog Objects")]
    public GameObject inGameCanvas;
    public GameObject DialogUI;
    public Image backgroundImage;
    public Image speakerImage;
    public TMP_Text speakerText;
    public Transform speakerEffectSpawnpoint;
    public TMP_Text dialogText;
    public Animator dialogButton;
    public GameObject backgroundTransitionPrefab;

    private string speakerPreviousSpriteName;
    private string backgroundImagePreviousName = "no background";
    private Animator speakerImageAnimator;
    private GameObject speakerEmotionEffectObject;

    [Header("Dialog Typing and Waiting Time")]
    public float typeLetterInterval = 0.075f;
    public float nextDialogWaitTime = 0.8f;
    public float backgroundTransitionMaxtWaitTime = 1f;
    public float backgroundTransitionCurrentWaitTime = 0f;

    [Header("Dialog Repeat Setting")]
    public bool isDialogRepeatable = false;
    public bool isDialogDisabledAfterFinish = false;
    private bool isDialogPlayed = false;

    [Header("Active End Dialog Object")]
    public GameObject activeAfterEndDialogObject;

    [Header("Dialog Data")]
    public DialogListClass dialogList = new DialogListClass();
    private Queue<string> dialogQueue = new Queue<string>();

    private MasterInput playerInput;

    private void Awake()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        speakerText.GetComponent<TMP_Text>();
        dialogText.GetComponent<TMP_Text>();
        speakerImage.GetComponent<Image>();
        dialogButton.GetComponent<Animator>();
        speakerImageAnimator = speakerImage.GetComponent<Animator>();
    }
    
    public void StartDialogCutscene()
    {
        if (isDialogPlayed == false)
        {
            isDialogPlayed = true;
            DialogUI.SetActive(true);
            SetupDialog();
            PlayNextDialog();
            GameController.TriggerCutscene();
        }
    }
    private void EndDialogCutscene()
    {
        DialogUI.SetActive(false);
        playerInput.Disable();
        RemoveControl();
        GameController.AllowMovement();
        DestroySpeakerEmotionEffectSprite();

        if (isDialogRepeatable == true)
        {
            isDialogPlayed = false;
        }
        if (isDialogDisabledAfterFinish == true)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void SetupDialog()
    {
        dialogIndex = 0;
        dialogSetIndex = 0;
        dialogQueue.Clear();
        for (int i = 0; i < dialogList.DialogSet[dialogSetIndex].DialogData.Count; i++)
        {
            dialogQueue.Enqueue(dialogList.DialogSet[dialogSetIndex].DialogData[i].dialogString);
        }
    }
    public void PlayNextDialog()
    {
        if (isWaitingAfterDialogEnd == false)
        {
            CheckDialogUiActivation();
            CheckDialogQueue();
            CheckDialogPlaying();
        }
    }
    private void CheckDialogUiActivation()
    {
        if (DialogUI.activeSelf == false)
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
        speakerText.text = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerString;
        SetSpeakerImage();
        SetBackgroundImage();
        PlaySpeakerAnimation();
        PlaySpeakerEmotionEffect();
        PlayDialogButtonConfirmAnimation();
        PlayDialogButtonTypeAnimation();
        StartCoroutine(TypeDialog(dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].dialogString));
    }
    private void SetSpeakerImage()
    {
        if (CheckIfSpeakerImageIsAvailable())
        {
            speakerImage.gameObject.SetActive(true);
            speakerImage.overrideSprite = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite;
        }
        else
        {
            speakerImage.gameObject.SetActive(false);
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
                backgroundImagePreviousName = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite.name;
            }

            dialogText.text = "";
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.overrideSprite = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite;
        }
        else if (!CheckIfBackgroundImageIsAvailable() && backgroundImagePreviousName != "no background")
        {
            CreateBackgroundTransition();
            backgroundImage.gameObject.SetActive(false);
            backgroundImagePreviousName = "no background";
        }
        else if (!CheckIfBackgroundImageIsAvailable() && backgroundImagePreviousName == "no background")
        {
            backgroundImage.gameObject.SetActive(false);
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
        GameObject backgroundTransition = Instantiate(backgroundTransitionPrefab);
        backgroundTransition.transform.SetParent(inGameCanvas.transform); 
        backgroundTransition.transform.SetAsLastSibling();
    }
    private bool CheckIfSpeakerImageIsAvailable()
    {
        return dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite != null;
    }
    private bool CheckIfBackgroundImageIsAvailable()
    {
        return dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite != null;
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
        speakerPreviousSpriteName = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite.name;
        print(speakerPreviousSpriteName);
    }
    private bool CheckIfSpeakerIsNew()
    {
        return dialogIndex > 0 && speakerPreviousSpriteName != dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite.name;
    }
    private bool CheckIfBackgroundIsNew()
    {
        return backgroundImagePreviousName != dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite.name;
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
            speakerEmotionEffectObject = Instantiate(dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerEmotionEffect);
            speakerEmotionEffectObject.transform.SetParent(speakerEffectSpawnpoint);
            speakerEmotionEffectObject.GetComponent<Animator>().SetTrigger(dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerEmotionEffect.name); ;
        }
    }
    private bool CheckIfSpeakerEmotionEffectIsAvailable()
    {
        return dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerEmotionEffect != null ;
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
        dialogText.text = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].dialogString;
        DialogEnd();
    }
    IEnumerator TypeDialog(string dialog)
    {
        canForceEndDialog = false;
        yield return new WaitForSeconds(backgroundTransitionCurrentWaitTime);

        dialogText.text = "";
        isDialogActive = true;
        canForceEndDialog = true;
        foreach (char letterOfDialog in dialog.ToCharArray())
        {
            yield return new WaitForSeconds(typeLetterInterval);
            dialogText.text += letterOfDialog;
            yield return null;
        }
        DialogEnd();
    }
    private void DialogEnd()
    {
        canForceEndDialog = false;
        dialogIndex++;

        StartCoroutine(DialogEndWaiting());
        ActiveObjectAtEndDialog();

        isDialogActive = false;
        dialogQueue.Dequeue();

        PlayDialogButtonWaitAnimation();
    }
    private void PlayDialogButtonTypeAnimation()
    {
        dialogButton.SetBool("isDialogButtonPlaying", true);
    }
    private void PlayDialogButtonWaitAnimation()
    {
        dialogButton.SetBool("isDialogButtonPlaying", false);
    }
    private void PlayDialogButtonConfirmAnimation()
    {
        dialogButton.SetTrigger("triggerDialogButtonConfirm");
    }

    IEnumerator DialogEndWaiting()
    {
        isWaitingAfterDialogEnd = true;
        yield return new WaitForSeconds(nextDialogWaitTime);
        isWaitingAfterDialogEnd = false;
    }
    private void ActiveObjectAtEndDialog()
    {
        if (activeAfterEndDialogObject != null)
        {
            activeAfterEndDialogObject.SetActive(true);
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
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlGeneral.NextDialog.performed += context => PlayNextDialog();
        playerInput.Enable();
    }
    private void RemoveControl()
    {
        playerInput.PlayerControlGeneral.NextDialog.performed -= context => PlayNextDialog();
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

        EditorGUI.BeginDisabledGroup(dialog.DialogUI.activeSelf == true);
        if (GUILayout.Button("Start Dialog")) {dialog.StartDialogCutscene();}
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(dialog.DialogUI.activeSelf == false);
        if (GUILayout.Button("Play Next Dialog")) {dialog.PlayNextDialog();}
        EditorGUI.EndDisabledGroup();
    }
}
#endif