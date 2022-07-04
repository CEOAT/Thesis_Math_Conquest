using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutsceneControllerDialog : MonoBehaviour
{
    [SerializeField] private int dialogIndex = 0;
    [SerializeField] private bool isDialogActive;
    [SerializeField] private int dialogSetIndex = 0;

    public GameObject DialogUI;
    public Image speakerImage;
    public TMP_Text speakerText;
    public TMP_Text dialogText;

    public DialogListClass dialogList = new DialogListClass();
    private Queue<string> dialogQueue = new Queue<string>();

    private MasterInput playerInput;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
        SetupDialog();
        
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
    private void SetupDialog()
    {
        dialogQueue.Clear();
        for (int i = 0; i < dialogList.DialogSet[dialogSetIndex].DialogData.Count; i++)
        {
            dialogQueue.Enqueue(dialogList.DialogSet[dialogSetIndex].DialogData[i].dialogString);
        }
    }
    private void OnEnable()
    {
        playerInput.Enable();
        PlayNextDialog();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }


    private void PlayNextDialog()
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
    private void EndDialogCutscene()
    {
        //start the game
        //or end the stage
        DialogUI.SetActive(false);
    }
    private void DialogStart()
    {
        speakerText.text = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerString;
        speakerImage.overrideSprite = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].speakerSprite;
        StartCoroutine(TypeDialog(dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].dialogString));
    }
    private void DialogForceEnd()
    {
        StopAllCoroutines();
        dialogText.text = dialogList.DialogSet[dialogSetIndex].DialogData[dialogIndex].dialogString;
        dialogIndex++;
        isDialogActive = false;
        dialogQueue.Dequeue();
    }
    IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        isDialogActive = true;
        foreach (char letterOfDialog in dialog.ToCharArray())
        {
            yield return new WaitForSeconds(0.1f);
            dialogText.text += letterOfDialog;
            yield return null;
        }
        dialogIndex++;
        isDialogActive = false;
        dialogQueue.Dequeue();
    }
}
