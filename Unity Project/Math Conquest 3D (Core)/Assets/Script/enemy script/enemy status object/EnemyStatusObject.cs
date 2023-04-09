using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusObject : MonoBehaviour
{
    public Transform enemyOwnDetailObject;
    [SerializeField] public float enemyUiPointHeight;
    [SerializeField] public float enemyUiPointZ;

    private void FixedUpdate()
    {
        DetailObjectFollow();
        DetailObjectCheckDestroy();
    }
    private void DetailObjectFollow()
    {
        if(enemyOwnDetailObject != null) 
        { 
            transform.position = enemyOwnDetailObject.position + new Vector3(0, enemyUiPointHeight, enemyUiPointZ);
        }
    }
    private void DetailObjectCheckDestroy()
    {
        if (enemyOwnDetailObject == null)
        {
            Destroy(this.gameObject);
        }
    }
}
