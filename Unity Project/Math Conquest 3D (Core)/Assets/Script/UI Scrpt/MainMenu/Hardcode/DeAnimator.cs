using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeAnimator : MonoBehaviour
{
   [SerializeField] private Animator _animator;

   public void DisableAnim()
   {
      _animator.enabled = false;
   }
}
