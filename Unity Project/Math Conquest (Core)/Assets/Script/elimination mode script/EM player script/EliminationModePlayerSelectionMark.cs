using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationModePlayerSelectionMark : MonoBehaviour
{
    public Transform enemyTransform;

    private void FixedUpdate()
    {
        if (enemyTransform == null)
        {
            return;
        }
        else
        {
            transform.position = enemyTransform.position + (Vector3.left * 2f) + (Vector3.back * 1f
                );
        }
    }
}