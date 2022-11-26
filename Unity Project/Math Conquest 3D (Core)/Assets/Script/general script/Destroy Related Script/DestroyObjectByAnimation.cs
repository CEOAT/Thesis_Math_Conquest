using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectByAnimation : MonoBehaviour
{
    public void DestroyOnAnimationEvent()
    {
        Destroy(this.gameObject);
    }
}