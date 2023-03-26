using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyComponentDropItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemDropPrefabList = new List<GameObject>();
    [HideIf("dropType", DropType.confirmDropEveryItem)] [SerializeField] private List<float> itemDropRateList = new List<float>();
    [SerializeField] private enum DropType
    {
        dropOnlyOneItem, randomDropEveryItem, confirmDropEveryItem
    }
    [SerializeField] private DropType dropType;
    [SerializeField] private float forceMinimum = 50f;
    [SerializeField] private float forceMaximum = 300f;
    private EnemyControllerMovement EnemyMovement;

    private void Start()
    {
        SetupComponent();
        SetupEvent();
    }
    private void SetupComponent()
    {
        EnemyMovement = GetComponent<EnemyControllerMovement>();
    }
    private void SetupEvent()
    {
        EnemyMovement.EventOnEnemyDead.AddListener(LoopDropItemInList);
    }

    private void OnDestroy() 
    {
        RemoveListener();
    }
    private void RemoveListener()
    {
        EnemyMovement.EventOnEnemyDead.RemoveListener(LoopDropItemInList);
    }

    private void LoopDropItemInList()
    {
        foreach(GameObject item in itemDropPrefabList)
        {
            int itemIndex = itemDropPrefabList.IndexOf(item);
            GameObject itemObject;

            if(CheckIfItemDrop(itemIndex, itemDropRateList[itemIndex]))
            {
                itemObject = Instantiate(itemDropPrefabList[itemIndex], transform.position, transform.rotation);
                itemObject.SetActive(true);
                AddForceToItem(itemObject.GetComponent<Rigidbody>());
                
                if(dropType == DropType.dropOnlyOneItem)
                { break; }
            }
        }
    }
    private bool CheckIfItemDrop(int itemIndex, float itemDropRate)
    {
        bool isItemDrop = false;
        float dropValue = Random.Range(0f, 100f);

        if(dropValue <= itemDropRate && dropType != DropType.confirmDropEveryItem)
        {
            isItemDrop = true;
        }
        else if(dropType == DropType.confirmDropEveryItem)
        {
            isItemDrop = true;
        }
        
        return isItemDrop;
    }
    private void AddForceToItem(Rigidbody itemRigidbody)
    {
        itemRigidbody.AddForce(RandomItemForceAngle() * RandomItemForceAmout());
    }
    private Vector3 RandomItemForceAngle()
    {
        int axisX = Random.Range(-90, -45);
        int axisY = Random.Range(0, 360);
        return new Vector3(axisX, axisY, 0);
    }
    private float RandomItemForceAmout()
    {
        float forceAmount;
        return forceAmount = Random.Range(forceMinimum, forceMaximum);
    }
}
