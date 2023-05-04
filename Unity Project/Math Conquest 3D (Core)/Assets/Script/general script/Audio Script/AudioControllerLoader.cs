using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControllerLoader : MonoBehaviour
{
    private float aduioVolumeMaster;
    private float audioVolumeSfx;
    private float audioVolumeAmbient;
    private float audioVolumeBgm;
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        LoadAudioSetting();
    }
    private void LoadAudioSetting()
    {
        audioMixer.SetFloat("Master_Volume", PlayerPrefs.GetFloat("Master_Volume", 0));
        audioMixer.SetFloat("SFX_Volume", PlayerPrefs.GetFloat("SFX_Volume", 0));
        audioMixer.SetFloat("Ambient_Volume", PlayerPrefs.GetFloat("Ambient_Volume", 0));
        audioMixer.SetFloat("BGM_Volume", PlayerPrefs.GetFloat("BGM_Volume", 0));
    }
}