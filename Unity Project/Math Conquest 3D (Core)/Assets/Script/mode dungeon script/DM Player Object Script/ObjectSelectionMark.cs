using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionMark : MonoBehaviour
{
    public Transform enemyTransform;

    private void Start()
    {
        enemyTransform.GetComponent<EnemyControllerStatus>().isEnemySelectedUI = true;
    }
    private void OnDestroy()
    {
        enemyTransform.GetComponent<EnemyControllerStatus>().isEnemySelectedUI = false;
    }

    private void FixedUpdate()
    {
        if (enemyTransform == null)
        {
            return;
        }
        else
        {
            transform.position = enemyTransform.position + (Vector3.left * 2f) + (Vector3.back * 1f);
        }
    }
}