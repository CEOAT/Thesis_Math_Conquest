using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerAttackShockwave : MonoBehaviour
{
    public Transform enemyTargetTransform;
    public string shockwaveAnswer;
    public float shockwaveSpeed = 1f;
    public float shockwaveDamage;
    public GameObject ShockwaveImpactParticle;

    private void Start()
    {
        CheckShockwaveFacing();
    }
    private void CheckShockwaveFacing()
    {
        if (enemyTargetTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (enemyTargetTransform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, enemyTargetTransform.position, Mathf.Clamp(Time.deltaTime, 0f, 1f) * shockwaveSpeed);

        //if (Mathf.Abs(transform.position.x - enemyTargetTransform.position.x) < 1f)
        //{
        //    enemyTargetTransform.GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(shockwaveAnswer, shockwaveDamage);
        //    Destroy(this.gameObject);
        //}
    }

    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            if (enemyTargetTransform.name == enemy.name)
            {
                enemyTargetTransform.GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(shockwaveAnswer, shockwaveDamage);
                CreateShockwaveImpactParticle();
                Destroy(this.gameObject);
            }
        }
    }
    private void CreateShockwaveImpactParticle()
    {
        GameObject shockwaveImpact = Instantiate(ShockwaveImpactParticle, transform.position, ShockwaveImpactParticle.transform.rotation);
        if (shockwaveImpact.transform.position.x > enemyTargetTransform.position.x)
        {
            shockwaveImpact.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (shockwaveImpact.transform.position.x < enemyTargetTransform.position.x)
        {
            shockwaveImpact.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        Destroy(shockwaveImpact, 1f);
    }
}