using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayAnimation : MonoBehaviour
{
    public string animationName;
    private void Start()
    {
        GetComponent<Animator>().Play(animationName);
    }
}
