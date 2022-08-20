using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonModePlayerControllerSwitchTarget : MonoBehaviour
{
    [SerializeField]private List<Transform> enemyList = new List<Transform>();

    public GameObject selectionMarkPrefab;
    private GameObject selectionMarkObject;
    [SerializeField] private Transform selectedEnemyObject;
    [SerializeField] private int selectedEnemyIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            AddEnemyToList(other.transform);
            SelectionMarkOnEnter();
            CheckEnemyIndex();
        }
    }
    private void AddEnemyToList(Transform enemy)
    {
        if (enemyList.Count == 0)
        {
            enemyList.Add(enemy.transform);
            selectedEnemyObject = enemy;
        }
        else if (enemyList.Count > 0)
        {
            if (enemyList.Contains(enemy) == false)
            {
                enemyList.Add(enemy.transform);
            }
        }
    }
    private void SelectionMarkOnEnter()
    {
        if (enemyList.Count == 1)
        {
            selectionMarkObject = Instantiate(selectionMarkPrefab);
            selectionMarkObject.GetComponent<ObjectSelectionMark>().enemyTransform = enemyList[0].transform;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveEnemyFromList(other.transform);
            RemoveSelectionMark();
            CheckEnemyIndex();
        }
    }
    private void RemoveEnemyFromList(Transform enemy)
    {
        enemyList.Remove(enemy);
    }
    private void RemoveSelectionMark()
    {
        if (enemyList.Count == 0)
        {
            selectedEnemyObject = null;
            Destroy(selectionMarkObject);
        }
        else if (enemyList.Count == 1)
        {
            selectedEnemyObject = enemyList[0];
            Destroy(selectionMarkObject);

            selectionMarkObject = Instantiate(selectionMarkPrefab);
            selectionMarkObject.GetComponent<ObjectSelectionMark>().enemyTransform = enemyList[0].transform;
        }
        else if (enemyList.Count > 1)
        {
            if (enemyList.Contains(selectedEnemyObject) == false)
            {
                selectedEnemyObject = null;
                Destroy(selectionMarkObject);

                selectionMarkObject = Instantiate(selectionMarkPrefab);
                selectionMarkObject.GetComponent<ObjectSelectionMark>().enemyTransform = enemyList[0].transform;
            }
        }
    }


    private void CheckEnemyIndex()  //keep checking index everytime enemy enter-leave trigger
    {
        selectedEnemyIndex = enemyList.IndexOf(selectedEnemyObject);
    }
    private void SelectEnemy()  //can be pressed when there 2 more target
    {

    }
}

