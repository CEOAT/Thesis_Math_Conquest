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
    [SerializeField] private List<AudioControllerSetting> volumeslider = new List<AudioControllerSetting>();

    private List<float> tempslideValue = new List<float>();

    private Resolution[] _resolutions;
    private List<FullScreenMode> _fullScreenModes;

    private List<Resolution> _filteredResolutions;

    private int _currentResolutionIndex = 0;

    private int _currentfullscreenIndex = 0;

    private GenericHoldButtonEvent _tempReset;

    private void Awake()
    {
      
    }

    void Start()
    {
        SetUpScreenSetting();
        SetTempStart();
    }

    private void SetUpScreenSetting()
    {
         _tempReset = applybutt.GetComponent<GenericHoldButtonEvent>();
        resolutionDropdown.ClearOptions();
        screenModeDropdown.ClearOptions();
        _resolutions = Screen.resolutions;
        
        List<string> optionFullscreen = new List<string>();
        _fullScreenModes = new List<FullScreenMode>();
        
        foreach (FullScreenMode VARIABLE in Enum.GetValues(typeof(FullScreenMode)))
        {
            //Filtering MaximizedWindows
            if (VARIABLE != FullScreenMode.MaximizedWindow)
            {
                _fullScreenModes.Add(VARIABLE);
                optionFullscreen.Add(VARIABLE.ToString());
            }
        }
        _currentfullscreenIndex = 0;
        foreach (FullScreenMode VARIABLE in Enum.GetValues(typeof(FullScreenMode)))
        {
            //Find CurrentScreenModeIndex
            if (Screen.fullScreenMode == VARIABLE)
            {
                screenModeDropdown.value = _currentfullscreenIndex;
                break;
            }
            _currentfullscreenIndex++;
        }
        //Set ScreenMode Value And current ScreenMode
        screenModeDropdown.AddOptions(optionFullscreen);
        screenModeDropdown.value = _currentfullscreenIndex;
        screenModeDropdown.RefreshShownValue();

        _filteredResolutions = new List<Resolution>();
        
        List<string> option = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            //16:9 Flitering 
            if (_resolutions[i].width % 16 == 0 && _resolutions[i].height % 9 == 0 &&_resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        for (int i = 0; i<_filteredResolutions.Count; i++)
        {
            string resolutiontxt = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + "@HZ"+
                                   _filteredResolutions[i].refreshRate ;
            //Add Custom Text To string list
            option.Add(resolutiontxt);
            //Get Current Resolution Index From Current Screen
            if (_filteredResolutions[i].width == Screen.currentResolution.width && _filteredResolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }
        // Set String List to drop Down
        resolutionDropdown.AddOptions(option);
        // Set Current Resolution Index From Current Screen
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void SetTempStart()
    {
        foreach (var SliderValue in volumeslider)
        {
            tempslideValue.Add(SliderValue.Silder.value);
        }
    }

    private void OnEnable()
    {
        try
        {
            int i = 0;
            foreach (var SliderValue in volumeslider)
            {
                tempslideValue[i]= SliderValue.Silder.value;
                i++;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Order Exception");
        }
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

    public void HackSetInteractable()
    {
        Resolution resolution = _filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width,resolution.height,_fullScreenModes[screenModeDropdown.value]);
        _currentResolutionIndex = resolutionDropdown.value;
        _currentfullscreenIndex =  screenModeDropdown.value;
        
        _tempReset.ResetDefaultState();
    }

    public void OnchangeSetting()
    {
        // Debug.Log("_currentResolutionIndex"+_currentResolutionIndex +"DropValue"+resolutionDropdown.value);
        // Debug.Log("_currentfullscreenIndex"+_currentfullscreenIndex +"DropValue"+screenModeDropdown.value);
        int i = 0;
        bool SliderChanged = false;
        try
        {
            _tempReset.ResetDefaultState();
            foreach (var SliderValue in volumeslider)
            {
                if (tempslideValue[i] < SliderValue.Silder.value ||
                    tempslideValue[i] > SliderValue.Silder.value)
                {
                    SliderChanged = true;
                    break;
                }
                i++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
           
        }
        if (_currentResolutionIndex != resolutionDropdown.value || screenModeDropdown.value != _currentfullscreenIndex ||SliderChanged)
        {
            applybutt.interactable = true;
        }
        else
        {
            applybutt.interactable = false;
        }
    }

}
