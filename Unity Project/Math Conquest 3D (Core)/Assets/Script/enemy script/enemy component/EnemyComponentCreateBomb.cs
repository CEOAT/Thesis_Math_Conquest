using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyComponentCreateBomb : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombStartForce;
    [SerializeField] private Vector3 bombSpawnOffset;

    [Header("Bomb Self Harm")] 
    [SerializeField] private bool isBombHarmEnemy;
    private Transform creatorTransform;

    [Header("Bomb Count Down")]
    [SerializeField] private bool isBombHasCountdown = true;
    [SerializeField] [ShowIf("isBombHasCountdown")] private float bombCountdownTime = 3f;

    [Header("Bomb Drop On Dead")]
    [SerializeField] private bool isDropBombOnDead = false;
    
    [Header("Bomb Constant Drop")]
    [SerializeField] private bool isConstantCreateBomb = false;
    [SerializeField] [ShowIf("isConstantCreateBomb")] [Range(1f, 5f)] private float bombDropInterval = 2f;
    
    private GameObject bombObject;
    private EnemyControllerMovement EnemyControl;
    private BombExplosion BombExplosion;

    private void Start()
    {
        SetupComponent();
        SetupBombSubcription();
    }
    private void SetupComponent()
    {
        EnemyControl = GetComponent<EnemyControllerMovement>();
    }
    private void SetupBombSubcription()
    {
        if(isDropBombOnDead)
        {
            EnemyControl.EventOnEnemyDead.AddListener(CreateBomb);
        }
        if(isConstantCreateBomb)
        {
            EnemyControl.EventOnEnemyStartChase.AddListener(StartConstantCreateBomb);
            EnemyControl.EventOnEnemyStopChase.AddListener(StopConstantCreateBomb);
        }
    }

    private void Destroy()
    {
        SetupBompUnsubcription();
    }
    private void SetupBompUnsubcription()
    {
        if(isDropBombOnDead)
        {
            EnemyControl.EventOnEnemyDead.RemoveListener(CreateBomb);
        }
        if(isConstantCreateBomb)
        {
            EnemyControl.EventOnEnemyStartChase.RemoveListener(StartConstantCreateBomb);
            EnemyControl.EventOnEnemyStopChase.RemoveListener(StopConstantCreateBomb);
        }
        StopConstantCreateBomb();
    }

    private void StartConstantCreateBomb()
    { 
        if(!IsInvoking("CreateBomb"))
            InvokeRepeating("CreateBomb", bombDropInterval, bombDropInterval);
    }
    private void StopConstantCreateBomb()
    {
        CancelInvoke("CreateBomb");
    }
    
    private void CreateBomb()
    {
        RefenceBomb();
        AssignBombProperty();
        AddForceToBomb();
    }
    private void RefenceBomb()
    {
        bombObject = Instantiate(bombPrefab, transform.position + bombSpawnOffset, transform.rotation);
        BombExplosion = bombObject.GetComponent<BombExplosion>();
    }
    private void AssignBombProperty()
    {
        if(!isBombHasCountdown)
        {
            BombExplosion.InvokeExplode(0, isBombHarmEnemy);
        }
        else
        {
            BombExplosion.InvokeExplode(bombCountdownTime, isBombHarmEnemy);
        }
    }
    private void AddForceToBomb()
    {
        BombExplosion.GetComponent<Rigidbody>().AddForce(Vector3.up * bombStartForce);
    }
}
