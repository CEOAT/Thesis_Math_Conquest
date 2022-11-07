using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusObject : MonoBehaviour
{
    public Transform enemyOwnDetailObject;

    private void Start()
    {
        transform.eulerAngles = new Vector3(35f, 0f, 0f);
    }

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
