using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerParticleEffect : MonoBehaviour
{
    public float particleLifeTime;
    public float particleLifeTimeReduce;
    private string particleAnimationName;
    public ParticleAnimation particleAnimation;
    public enum ParticleAnimation
    {
        scaleDown
    };

    private void Start()
    {
        SetupParticleProperty();
        StartCoroutine(ParticleLifeTime());
    }
    private void SetupParticleProperty()
    {
        switch (particleAnimation)
        {
            case ParticleAnimation.scaleDown:
                {
                    particleAnimationName = "Particle Scale Down";
                    break;
                }
        }
    }
    private IEnumerator ParticleLifeTime()
    {
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(particleLifeTime - particleLifeTimeReduce);
        GetComponent<Animator>().SetTrigger($"trigger {particleAnimationName}");
    }
}