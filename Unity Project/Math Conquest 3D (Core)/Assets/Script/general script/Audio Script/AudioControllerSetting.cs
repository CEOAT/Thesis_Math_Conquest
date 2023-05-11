using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControllerSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string audioGroupName;
    private float audioValue;
    private Slider silder;

    public Slider Silder => silder;

    private void Awake()
    {
        SetupComponent();
    }

    private void OnEnable()
    {
        SetupAudioSetting();
    }
    private void SetupComponent()
    {
        silder = GetComponent<Slider>();
    }
    private void SetupAudioSetting()
    {
        audioValue = PlayerPrefs.GetFloat(audioGroupName, 0);
        silder.value = PlayerPrefs.GetFloat(audioGroupName + "_Silder", 1);;
    }

    public void SetAudioVolume()
    {
        audioValue = Mathf.Log10(silder.value) * 20;
        audioMixer.SetFloat(audioGroupName, audioValue);
    }

    private void OnDisable() 
    {
        SaveAudioSetting();
    }
    public void SaveAudioSetting()
    {
        PlayerPrefs.SetFloat(audioGroupName, audioValue);
        PlayerPrefs.SetFloat(audioGroupName + "_Silder", silder.value);
    }
}