using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExplorationModeItemObject : MonoBehaviour
{
    private enum ItemType
    {
        retoreHealth, fullRestoreHealth, increaseMaxHealth, increaseMoveSpeed, increaseAttackDamage
    }
    [SerializeField] private ItemType itemType;
    [SerializeField, HideIf("itemType", ItemType.fullRestoreHealth)] private float itemValue = 10;
    [SerializeField] private GameObject itemPickupEffectPrefab;
    [SerializeField] private float itemPickUpEffectHeight;
    private ExplorationModePlayerBuff PlayerBuff;

    private void OnCollisionEnter(Collision ground) 
    {
        if(ground.collider.tag == "Untagged"
        || ground.collider.tag == "Ground"
        || ground.collider.tag == "Environment")
        {
            ChangeColliderToTrigger();
        }
    }
    private void ChangeColliderToTrigger()
    {
        GetComponent<Collider>().isTrigger = true;
        Destroy(GetComponent<Rigidbody>());
    }

    private void OnTriggerEnter(Collider player)
    {
        if(player.CompareTag("Player"))
        {
            PlayerBuff = player.GetComponent<ExplorationModePlayerBuff>();
            SelectItemType();
            CreatePickUpEffect();
            Destroy(this.gameObject);
        }
    }
    private void SelectItemType()
    {
        if(itemType == ItemType.retoreHealth)
        {
            PlayerBuff.RestoreHealth(itemValue);
        }
        else if(itemType == ItemType.increaseMaxHealth)
        {
            PlayerBuff.IncreaseMaxHealth(itemValue);
        }
        else if(itemType == ItemType.fullRestoreHealth)
        {
            PlayerBuff.RestoreFullHealth();
        }
        else if(itemType == ItemType.increaseMoveSpeed)
        {
            PlayerBuff.IncreaseMoveSpeed(itemValue);
        }
        else if(itemType == ItemType.increaseAttackDamage)
        {
            PlayerBuff.IncreaseDamage(itemValue);
        }
    }
    private void CreatePickUpEffect()
    {
        Destroy(Instantiate(itemPickupEffectPrefab, 
        PlayerBuff.transform.position + (Vector3.up * itemPickUpEffectHeight), 
        itemPickupEffectPrefab.transform.rotation,
        PlayerBuff.transform
        ), 3f);
    }
}