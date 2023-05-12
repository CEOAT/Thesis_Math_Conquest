using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VectorLerpManager : MonoBehaviour
{
    [SerializeField] private List<VectorLerp> platforms = new List<VectorLerp>();
    [SerializeField] private GameObject Canvas;
    [SerializeField] private TextMeshProUGUI LerpValue;
    [SerializeField] private Slider LerpSlider;
    [SerializeField] private GameObject cameraPuzzle;

    private VectorLerp tempPlatform;
    private MasterInput playerInput;
    private ExplorationModeGameController _playeGameController;
    #region CapSual

    public bool interactable;
    public List<VectorLerp> Platforms => platforms;

    public MasterInput PlayerInput
    {
        get => playerInput;
        set => playerInput = value;
    }

    #endregion

    

    void Start()
    {
        var tempPlayer = FindObjectOfType<ExplorationModeGameController>();
        _playeGameController = tempPlayer.GetComponent<ExplorationModeGameController>();
        Initconfiguration();
    }

     public void SetUpInput()
    {
        playerInput = new MasterInput();
        playerInput.VectorPuzzleControl.Interact.performed += context => EnablePuzzle();
        playerInput.VectorPuzzleControl.CloseWindow.performed += context => DisablePuzzle();
    }

     private void EnablePuzzle()
     {
         if (!interactable) return;
         _playeGameController.TriggerCutscene();
         Canvas.SetActive(true);
         cameraPuzzle.SetActive(true);
         tempPlatform.OnFocus(tempPlatform.isFocus);
     }
     private void DisablePuzzle()
     {
         Canvas.SetActive(false);
         cameraPuzzle.SetActive(false);
         tempPlatform.OnFocus(false);
         _playeGameController.AllowMovement();
     }

    public void OnChangeSlider()
    {
        LerpValue.text = $"t = {LerpSlider.value.ToString("f1")}";
        tempPlatform.T = LerpSlider.value;
    }

    public void FindCurrentPlatform()
    {
        foreach (var VARIABLE in platforms)
        {
            if (VARIABLE.isFocus)
            {
                tempPlatform = VARIABLE;
                LerpSlider.value = VARIABLE.T;
            }
        }
    }

   

    private void Initconfiguration()
    {
        
    }


}
