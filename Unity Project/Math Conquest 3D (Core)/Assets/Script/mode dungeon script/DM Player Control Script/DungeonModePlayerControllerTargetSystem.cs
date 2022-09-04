using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonModePlayerControllerTargetSystem : MonoBehaviour
{
    [SerializeField]private List<Transform> enemyList = new List<Transform>();
    private LayerMask playerAttackLayerMask;
    private DungeonModePlayerControllerAttack AttackSystem;

    public GameObject selectionMarkPrefab;
    private GameObject selectionMarkObject;

    public Transform selectedEnemyObject;
    [SerializeField] private int selectedEnemyIndex;

    public Transform targetAreaCenterPointObject;
    public float targetAreaXValue;
    public float targetAreaYValue;

    private void Start()
    {
        SetupComponent();
        InvokeRepeating("TargetEnemy", 0.5f, 0.5f);
    }
    private void SetupComponent()
    {
        AttackSystem = GetComponent<DungeonModePlayerControllerAttack>();
        playerAttackLayerMask = AttackSystem.playerAttackLayerMask;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(targetAreaCenterPointObject.position.x,targetAreaCenterPointObject.position.y, 0),
            new Vector3(targetAreaXValue, targetAreaYValue, 0f));
    }
    private void TargetEnemy()
    {
        CheckEnemyInArea();
        CheckEnemyIndex();
        CreateSelectionMark();
    }
    private void CheckEnemyInArea()
    {
        Collider2D[] enemiesInTargetArea = Physics2D.OverlapBoxAll(
            new Vector2(targetAreaCenterPointObject.position.x, targetAreaCenterPointObject.position.y),    //center point of gizmos (camera)
            new Vector2(targetAreaXValue, targetAreaYValue),                                                //size of gizmos
            0f,                                                                                             //angle of box
            playerAttackLayerMask);                                                                         //get mask from player attack script ("enemy" layer)

        foreach (Collider2D enemy in enemiesInTargetArea)
        {
            if (enemyList.Count == 0)
            {
                enemyList.Add(enemy.transform);
            }
            if (enemyList.Count > 0)
            {
                if (enemyList.Contains(enemy.transform) == false)
                {
                    enemyList.Add(enemy.transform);
                }
            }
        }

        if (enemiesInTargetArea.Length < enemyList.Count)
        {
            int enemyListCount = enemyList.Count;
            for (int i = enemyListCount; i > 0; i--)
            {
                if (enemiesInTargetArea.Equals(enemyList[i - 1].transform) == false)
                {
                    enemyList.Remove(enemyList[i - 1].transform);
                }
            }
        }
    }
    private void CheckEnemyIndex()  //keep checking index everytime enemy enter-leave trigger
    {
        if (enemyList.Count == 0)
        {
            selectedEnemyIndex = -1;
        }
        else if (enemyList.Count == 1)
        {
            selectedEnemyIndex = 0;
        }
    }
    private void CreateSelectionMark()
    {
        if (enemyList.Count == 0)
        {
            if (selectionMarkObject != null)
            {
                selectedEnemyObject = null;
                Destroy(selectionMarkObject);
            }
        }
        else if (enemyList.Count == 1)
        {
            if (selectionMarkObject == null)
            {
                selectedEnemyObject = enemyList[0].transform;
                selectionMarkObject = Instantiate(selectionMarkPrefab);
                selectionMarkObject.GetComponent<ObjectSelectionMark>().enemyTransform = selectedEnemyObject;
            }
        }
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
            if (selectedEnemyIndex + 1 > enemyList.Count)
            {
                selectedEnemyIndex = 0;
                return;
            }
            selectedEnemyIndex++;
        }
    }
}

