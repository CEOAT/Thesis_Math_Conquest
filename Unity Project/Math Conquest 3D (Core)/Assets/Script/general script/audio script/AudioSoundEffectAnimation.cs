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

        foreach (AnimationClip animation in audioList.animationClip)
        {
            if (currentAnimation == animation.name)
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
    public List<AnimationClip> animationClip;
    public List<AudioClip> soundEffectList;
}