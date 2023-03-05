using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [Header("Explosion Setting")]
    [SerializeField] public float bombExplodeDamage = 20f;
    [SerializeField] public float bombExplodeRange = 5f;
    [SerializeField] public float bombExplodeForce = 300f;

    [Header("Particle On Explosion")]
    [SerializeField] private GameObject particlePrefab;
    private bool isBombHarmEnemy;

    public void InvokeExplode(float bombExplodeCountdown, bool isBombHarmEnemy)
    {
        this.isBombHarmEnemy = isBombHarmEnemy;
        Invoke("Explode", bombExplodeCountdown);
    }
    
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplodeRange);
    }
    public void Explode()
    {
        this.isBombHarmEnemy = isBombHarmEnemy;
        Collider[] targetInExplosionCircle = Physics.OverlapSphere(
                transform.position,
                bombExplodeRange,
                LayerMask.GetMask("Enemy","Player"));
        
        foreach (Collider target in targetInExplosionCircle)
        {
            ApplyDamageToTarget(target);
            ApplyForceToTarget(target);
        }

        Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    private void ApplyDamageToTarget(Collider target)
    {
        if(target.tag == "Player")
        {
            target.GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(bombExplodeDamage/2);
        }
        else if(target.tag == "Enemy" && isBombHarmEnemy)
        {
            target.GetComponent<EnemyControllerStatus>().EnemyTakenDamage(bombExplodeDamage);
        }
    }
    private void ApplyForceToTarget(Collider target)
    {
        if(target.GetComponent<Rigidbody>() != null)
        {
            target.GetComponent<Rigidbody>().AddExplosionForce(bombExplodeForce, transform.position + (Vector3.up * 3f), bombExplodeRange);
        }
    }
}