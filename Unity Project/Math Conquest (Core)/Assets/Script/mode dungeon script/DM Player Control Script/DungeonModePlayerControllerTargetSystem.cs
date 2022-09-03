using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonModePlayerControllerTargetSystem : MonoBehaviour
{
    [SerializeField]private List<Transform> enemyList = new List<Transform>();

    public GameObject selectionMarkPrefab;
    private GameObject selectionMarkObject;
    public Transform selectedEnemyObject;
    [SerializeField] private int selectedEnemyIndex;

    public Transform targetAreaCenterPointObject;
    public float targetAreaXValue;
    public float targetAreaYValue;

    private void Start()
    {
        InvokeRepeating("TargetEnemyInArea", 0.5f, 0.5f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(new Vector3(targetAreaCenterPointObject.position.x,targetAreaCenterPointObject.position.y, 0),
            new Vector3(targetAreaXValue, targetAreaYValue, 0.5f));
    }
    private void TargetEnemyInArea()
    {
        print("repeating");
    }


    private void CheckEnemyIndex()  //keep checking index everytime enemy enter-leave trigger
    {
        selectedEnemyIndex = enemyList.IndexOf(selectedEnemyObject);
    }
    private void SwitchEnemy()  //can be pressed when there 2 more target
    {
        //increase index number
        //if there's more than 1 enemy, increase index and loop to 0.
        //if -1 (no enemy) just return

        if (enemyList.Count <= 1)
        {
            return;
        }
        else if (enemyList.Count > 1)
        {
            if (selectedEnemyIndex + 2 > enemyList.Count)
            {
                selectedEnemyIndex++;
                return;
            }
            selectedEnemyIndex++;
        }
    }
}

