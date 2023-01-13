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
    [SerializeField] private int dialogSetIndex = 0;

    [Header("Game Controller Object")]
    public ExplorationModeGameController GameController;

    [Header("Dialog Objects")]
    public GameObject DialogUI;
    public Image backgroundImage;
    public Image speakerImage;
    public TMP_Text speakerText;
    public TMP_Text dialogText;
    public Animator dialogButton;

    private string speakerPreviousName;
    private Animator speakerImageAnimator;
    private GameObject speakerEmotionEffectObject;

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
        GameController.AllowMovement();

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
        CheckDialogUiActivation();
        CheckDialogQueue();
        CheckDialogPlaying();
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
        if (isDialogActive == false)
        {
            DialogStart();
        }

        else if (isDialogActive == true)
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
        if (CheckIfBackgroundImageIsAvailable())
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.overrideSprite = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite;
        }
        else
        {
            backgroundImage.gameObject.SetActive(false);
        }
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
        speakerPreviousName = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerString;
    }
    private bool CheckIfSpeakerIsNew()
    {
        return dialogIndex > 0 && speakerPreviousName != dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerString;
    }
    private void PlaySpeakerEmotionEffect()
    {
        if (CheckIfSpeakerImageIsAvailable() == false)
        {
            return;
        }
        if (CheckIfSpeakerEmotionEffectIsAvailable())
        {
            if (speakerEmotionEffectObject != null)
            {
                Destroy(speakerEmotionEffectObject);
            }
            speakerEmotionEffectObject = Instantiate(dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerEmotionEffect);
        }
    }
    private bool CheckIfSpeakerEmotionEffectIsAvailable()
    {
        return dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerEmotionEffect != null ;
    }

    private void DialogForceEnd()
    {
        StopAllCoroutines();
        dialogText.text = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].dialogString;
        DialogEnd();
    }
    IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        isDialogActive = true;
        foreach (char letterOfDialog in dialog.ToCharArray())
        {
            yield return new WaitForSeconds(0.075f);
            dialogText.text += letterOfDialog;
            yield return null;
        }
        DialogEnd();
    }
    private void DialogEnd()
    {
        dialogIndex++;

        StartCoroutine(DialogEndWaiting());
        ActiveObjectAtEndDialog();

        isDialogActive = false;
        dialogQueue.Dequeue();
    }
    private void DialogButtonPlay()
    {
        dialogButton.SetBool("isDialogButtonPlaying", true);
    }
    private void DialogButtonStop()
    {
        dialogButton.SetBool("isDialogButtonPlaying", false);
    }

    IEnumerator DialogEndWaiting()
    {
        yield return new WaitForSeconds(0.8f);
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
            playerInput.Enable();
            StartDialogCutscene();
        }
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlGeneral.NextDialog.performed += context => PlayNextDialog();
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