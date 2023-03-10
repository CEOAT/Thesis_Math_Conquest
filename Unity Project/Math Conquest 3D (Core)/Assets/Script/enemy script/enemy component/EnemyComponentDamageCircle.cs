using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyComponentDamageCircle : MonoBehaviour
{
    [SerializeField] private float damagecircleSize;
    [SerializeField] private float damagecircleDamage;
    [SerializeField] private GameObject damagecirclePrefab;
    [SerializeField] private bool isDamageCircleActiveAllTime = false;
    private GameObject damagecircleObject;

    private EnemyControllerMovement EnemyController;

    private void Start() 
    {
        SetupComponent();
        SetupEvent();
        SetupDamageCircle();
    }
    private void SetupComponent()
    {
        EnemyController = GetComponent<EnemyControllerMovement>();
    }
    private void SetupEvent()
    {
        if(isDamageCircleActiveAllTime == false)
        {
            EnemyController.EventOnEnemyStartChase.AddListener(ActiveDamageCircle);
            EnemyController.EventOnEnemyStopChase.AddListener(DeactiveDamageCircle);
        }
    }
    private void SetupDamageCircle()
    {
        damagecircleObject = Instantiate(damagecirclePrefab, transform.GetChild(1).position, transform.rotation);
        damagecircleObject.transform.SetParent(transform.GetChild(1));
        damagecircleObject.GetComponent<DamageCircle>().SetupDamageCircle(damagecircleSize, damagecircleDamage);

        if(isDamageCircleActiveAllTime == false)
        {
            damagecircleObject.SetActive(false);
        }
    }

    private void OnDestroy() 
    {
        UnsubscribeEvent();
    }
    private void UnsubscribeEvent()
    {
        if(isDamageCircleActiveAllTime == false)
        {
            EnemyController.EventOnEnemyStartChase.RemoveListener(ActiveDamageCircle);
            EnemyController.EventOnEnemyStopChase.RemoveListener(DeactiveDamageCircle);
        }
    }

    private void ActiveDamageCircle()
    {
        damagecircleObject.SetActive(true);
    }
    private void DeactiveDamageCircle()
    {
        damagecircleObject.SetActive(false);
    }
}