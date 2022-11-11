using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoundEffectAnimation : MonoBehaviour
{
    public AudioSoundEffectAnimationListClass audioList = new AudioSoundEffectAnimationListClass();

    [SerializeField] private string currentAnimation;
    private int animationNameIndex;
    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioOnAnimation() // run this method in animation frame event
    {
        currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        animationNameIndex = 0;

        foreach (string animation in audioList.animationName)
        {
            if (currentAnimation == animation)
            {
                audioSource.clip = audioList.soundEffectList[animationNameIndex];
                audioSource.Play();
                break;
            }
            animationNameIndex++;
        }
    }
}

[System.Serializable]
public class AudioSoundEffectAnimationListClass
{
    public List<string> animationName;
    public List<AudioClip> soundEffectList;
}