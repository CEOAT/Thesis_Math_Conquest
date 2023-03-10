using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage = 5f;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float bulletDestroyTime = 3f;
    [SerializeField] private GameObject bulletDestroyParticlePrefab;
    private Transform playerTransform; 
    
    [SerializeField] private enum BulletType
    {
        chaser,
        linear
    }
    [SerializeField] private BulletType bulletType;

    private void Start() 
    {
        SetupButlletDestroyTime();
    }
    private void SetupButlletDestroyTime()
    {
        StartCoroutine(DestroyBullet());
    }
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletDestroyTime);
        CreateDestroyParticle();
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    public void SetupBulletVariable(float rBulletDamage, Transform rPlayerTransform)
    {
        bulletDamage = rBulletDamage;
        playerTransform = rPlayerTransform;
        transform.LookAt(playerTransform);
    }

    private void FixedUpdate()
    {
        BulletMove();
    }
    private void BulletMove()
    {
        if(bulletType == BulletType.chaser)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, bulletSpeed * 0.001f);
        }        
        else if(bulletType == BulletType.linear)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        }
    }

    private void OnTriggerEnter(Collider player) 
    {
        if(player.CompareTag("Player"))
        {
            player.GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(bulletDamage);
            CreateDestroyParticle();
            Destroy(this.gameObject);
        }
    }

    private void CreateDestroyParticle()
    {
        if(bulletDestroyParticlePrefab != null)
        {
            Destroy(Instantiate(bulletDestroyParticlePrefab, transform.position, bulletDestroyParticlePrefab.transform.rotation), 1.5f);
        }
    }
}