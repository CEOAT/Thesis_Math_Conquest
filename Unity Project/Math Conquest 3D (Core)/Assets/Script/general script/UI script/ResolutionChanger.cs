using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChanger : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField] private Button applybutt; 

    private Resolution[] _resolutions;
    private List<FullScreenMode> _fullScreenModes;

    private List<Resolution> _filteredResolutions;

    private int _currentResolutionIndex = 0;

    private int _currentfullscreenIndex = 0;

    private GenericHoldButtonEvent _tempReset;
  
    void Start()
    {
        _tempReset = applybutt.GetComponent<GenericHoldButtonEvent>();
        resolutionDropdown.ClearOptions();
        screenModeDropdown.ClearOptions();
        
        _resolutions = Screen.resolutions;
        
        List<string> optionFullscreen = new List<string>();
        _fullScreenModes = new List<FullScreenMode>();
        foreach (FullScreenMode VARIABLE in Enum.GetValues(typeof(FullScreenMode)))
        {
            if (Screen.fullScreenMode == VARIABLE) screenModeDropdown.value = _currentResolutionIndex;
            
            if (VARIABLE != FullScreenMode.MaximizedWindow)
            {
                _fullScreenModes.Add(VARIABLE);
                optionFullscreen.Add(VARIABLE.ToString());
            }

            _currentResolutionIndex++;
        }

        _currentResolutionIndex = screenModeDropdown.value;
        screenModeDropdown.AddOptions(optionFullscreen);
        _filteredResolutions = new List<Resolution>();
        List<string> option = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].width % 16 == 0 && _resolutions[i].height % 9 == 0 &&_resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        for (int i = 0; i<_filteredResolutions.Count; i++)
        {
            string resolutiontxt = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + "@HZ"+
                                   _filteredResolutions[i].refreshRate ;
            option.Add(resolutiontxt);
            if (_filteredResolutions[i].width == Screen.currentResolution.width && _filteredResolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(option);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        Resolution resolution = _filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width,resolution.height,_fullScreenModes[screenModeDropdown.value]);
        _currentResolutionIndex = resolutionDropdown.value;
        _currentfullscreenIndex =  screenModeDropdown.value;
        
        _tempReset.ResetDefaultState();
        applybutt.interactable = false;
       
    }

    public void OnchangeSetting()
    {
        // Debug.Log("_currentResolutionIndex"+_currentResolutionIndex +"DropValue"+resolutionDropdown.value);
        // Debug.Log("_currentfullscreenIndex"+_currentfullscreenIndex +"DropValue"+screenModeDropdown.value);
        _tempReset.ResetDefaultState();
        if (_currentResolutionIndex != resolutionDropdown.value || screenModeDropdown.value != _currentfullscreenIndex)
        {
            applybutt.interactable = true;
        }
        else
        {
            applybutt.interactable = false;
        }
    }

}
