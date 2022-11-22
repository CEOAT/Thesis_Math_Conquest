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

    [Header("Dialog Repeat Setting")]
    public bool isDialogRepeatable = false;
    public bool isDialogDisabledAfterFinish = false;
    private bool isDialogPlayed = false;

    [Header("Dialog Data")]
    public DialogListClass dialogList = new DialogListClass();
    private Queue<string> dialogQueue = new Queue<string>();

    private MasterInput playerInput;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        speakerImage.GetComponent<Image>();
        speakerText.GetComponent<TMP_Text>();
        dialogText.GetComponent<TMP_Text>();
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlGeneral.NextDialog.performed += context => PlayNextDialog();
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
        if (DialogUI.activeSelf == false)
        {
            return;
        }

        if (dialogQueue.Count == 0)
        {
            EndDialogCutscene();
            return;
        }

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
        DialogStartSetSpeakerImage();
        DialogStartSetBackgroundImage();
        StartCoroutine(TypeDialog(dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].dialogString));
    }
    private void DialogStartSetSpeakerImage()
    {
        if (dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite != null)
        {
            speakerImage.gameObject.SetActive(true);
            speakerImage.overrideSprite = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite;
        }
        else
        {
            speakerImage.gameObject.SetActive(false);
        }
    }
    private void DialogStartSetBackgroundImage()
    {
        if (dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite != null)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.overrideSprite = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].backgroundSprite;
        }
        else
        {
            backgroundImage.gameObject.SetActive(false);
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
        isDialogActive = false;
        dialogQueue.Dequeue();
    }

    IEnumerator DialogEndWaiting()
    {
        yield return new WaitForSeconds(0.8f);
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            playerInput.Enable();
            StartDialogCutscene();
        }
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