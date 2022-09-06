using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionMark : MonoBehaviour
{
    public Transform enemyTransform;
    private bool isObjectDeativated = false;

    private void Start()
    {
        enemyTransform.GetComponent<EnemyControllerStatus>().isEnemySelectedUI = true;
    }

    private void FixedUpdate()
    {
        if (enemyTransform == null && isObjectDeativated == false)
        {
            isObjectDeativated = true;
            enemyTransform.GetComponent<EnemyControllerStatus>().isEnemySelectedUI = false;
            Destroy(this.gameObject);
            return;
        }
        else
        {
            transform.position = enemyTransform.position + (Vector3.left * 2f) + (Vector3.back * 1f);
        }
    }
}