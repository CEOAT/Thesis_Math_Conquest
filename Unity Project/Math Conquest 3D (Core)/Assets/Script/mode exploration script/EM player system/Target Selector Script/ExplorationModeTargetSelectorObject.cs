using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeTargetSelectorObject : MonoBehaviour
{
    private Transform enemySelected;

    private void Start()
    {
        transform.eulerAngles = new Vector3(35,0,0);
    }
    private void Update()
    {
        if (enemySelected != null)
        {
            transform.position = enemySelected.position;
        }
    }
}
