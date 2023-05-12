using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class VectorAddForceManager : MonoBehaviour
{
    private MasterInput PlayerInput;
    private ExplorationModeGameController _playeGameController;

    [SerializeField] private AddForce puzzleBall;
    [SerializeField] private TextMeshProUGUI AddForcetext;
    [SerializeField] private GameObject CanvasPuzzle;
    [SerializeField] private TMP_InputField vectorX;
    [SerializeField] private TMP_InputField vectorZ;
    [SerializeField] private GameObject Axis;
    [SerializeField] private GameObject CameraPuzzle;

    public MasterInput thisPlayerInput
    {
        get => PlayerInput;
        set => PlayerInput = value;
    }
    public bool Interable;

    private void Awake()
    {
        SetUpInput();
    }

    private void Start()
    {
        var tempPlayer = FindObjectOfType<ExplorationModeGameController>();
        _playeGameController = tempPlayer.GetComponent<ExplorationModeGameController>();
    }

    private void Update()
    {
        AddForcetext.text = $"Vector Force = ({puzzleBall.Force.x},0,{puzzleBall.Force.z}) \n Magnitude limit = 2 ";
    }

    // Start is called before the first frame update
    public void EnablePuzzle()
    {
        if (!Interable) return;
        CameraPuzzle.SetActive(true);
        Axis.SetActive(true);
        _playeGameController.TriggerCutscene();
        puzzleBall.enabled = true;
        CanvasPuzzle.SetActive(true);
        PlayerInput.Enable();
    }

    public void DisablePuzzle()
    {
        Interable = true;
        CameraPuzzle.SetActive(false);
        Axis.SetActive(false);
        _playeGameController.AllowMovement();
        CanvasPuzzle.SetActive(false);
        puzzleBall.enabled = false;
    }

    public void EndPuzzle()
    {
        Interable = false;
        CameraPuzzle.SetActive(false);
        Axis.SetActive(false);
        _playeGameController.AllowMovement();
        CanvasPuzzle.SetActive(false);
        puzzleBall.enabled = false;
        PlayerInput.Disable();
        this.enabled = false;
    }

    private void SetUpInput()
    {
        PlayerInput = new MasterInput();
        PlayerInput.VectorPuzzleControl.Interact.performed += context => EnablePuzzle();
        PlayerInput.VectorPuzzleControl.ConfirmAnswer.performed += context => confirmValue();
        PlayerInput.VectorPuzzleControl.CloseWindow.performed += context => DisablePuzzle();
    }

    public void confirmValue()
    {
        float x, z;
        Regex regex = new Regex(@"^-?\d+(\.\d+)?$");
        if (vectorX.text == "" || vectorZ.text == "")
        {
            x = 0f;
            z = 0f;
            vectorX.text = x.ToString();
            vectorZ.text = z.ToString();
        }
        if (!regex.IsMatch(vectorX.text)||!regex.IsMatch(vectorZ.text))
        {
            x = 0f;
            z = 0f;
            vectorX.text = x.ToString();
            vectorZ.text = z.ToString();
        }
        else
        {
            x = float.Parse(vectorX.text);
            z = float.Parse(vectorZ.text);
        }
        var newVec = new Vector3(Mathf.Clamp(x,-2f,2f),0,Mathf.Clamp(z,-2f,2f));
        puzzleBall.Force = newVec ;
    }
    
}
