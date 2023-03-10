using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircle : MonoBehaviour
{
    private float circleDamage;
    [SerializeField] private GameObject circleActivateParticle;
    [SerializeField] private GameObject circleDeactivateParticle;
    private SphereCollider sphereCollider;
    private Collider playerCollider;

    // called from enemy damage circle component, the script can be used seperatedly from enemy component
    public void SetupDamageCircle(float damagecircleSize, float damagecircleDamage)
    {
        SetupEnemyComponent();
        circleDamage = damagecircleDamage;
        transform.localScale = new Vector3(damagecircleSize, damagecircleSize, damagecircleSize);
    }
    private void SetupEnemyComponent()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.center = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        SetupEnemyComponent();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            playerCollider = other;
            StartCoroutine("DamagePlayer");
        }
    }
    public IEnumerator DamagePlayer()
    {
        if(playerCollider.TryGetComponent<ExplorationModePlayerHealth>( out ExplorationModePlayerHealth playerHealth))
        {
            playerHealth.PlayerTakenDamage(circleDamage);
        }

        sphereCollider.center = new Vector3(0, -30f, 0);
        yield return new WaitForSeconds(2.15f);
        sphereCollider.center = new Vector3(0, 0, 0);
    }

    private void OnEnable() 
    {
        if(circleActivateParticle != null)
        {
            Destroy(Instantiate(circleActivateParticle, transform), 3f);
        }
    }
    private void OnDisable() 
    {
        if(circleDeactivateParticle != null)
        {
            Destroy(Instantiate(circleDeactivateParticle, transform, transform), 3f);
        }

        StopAllCoroutines();
        sphereCollider.center = new Vector3(0, 0, 0);
    }
}