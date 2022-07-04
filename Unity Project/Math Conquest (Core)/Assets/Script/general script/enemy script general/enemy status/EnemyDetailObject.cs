using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetailObject : MonoBehaviour
{
    public Transform enemyOwnDetailObject;

    private void FixedUpdate()
    {
        DetailObjectFollow();
        DetailObjectCheckDestroy();
    }
    private void DetailObjectFollow()
    {
        if(enemyOwnDetailObject != null) 
        { 
            transform.position = enemyOwnDetailObject.position + Vector3.up * 1.3f;
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
