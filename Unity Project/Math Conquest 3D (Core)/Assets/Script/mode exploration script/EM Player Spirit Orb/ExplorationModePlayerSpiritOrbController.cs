using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerSpiritOrbController : MonoBehaviour
{
    [Header("Transform")]
    public Transform playerTransform;
    public Transform spiritFollowPointTransform;
    public Transform spiritFollowPointCurrent;

    [Header("Spirit Sprite")]
    public SpriteRenderer SpiritSprite;

    private void Start()
    {
        spiritFollowPointCurrent = spiritFollowPointTransform;
        InvokeRepeating("CheckFollowPoint", 0f, 1f);
    }
    public void CheckFollowPoint()
    {
        spiritFollowPointCurrent = spiritFollowPointTransform;
    }

    private void FixedUpdate()
    {
        SpiritMoveToFollowPoint();
        CheckFacing();
        CheckPlayerDistance();
    }
    private void SpiritMoveToFollowPoint()
    {
        transform.position = Vector3.Lerp(transform.position, spiritFollowPointCurrent.position, Mathf.Clamp(Time.deltaTime, 0f, 1f) * 1.5f);
    }
    private void CheckFacing()
    {
        if (playerTransform.position.x > transform.position.x)
        {
            SpiritSprite.flipX = false;
        }
        else if (playerTransform.position.x < transform.position.x)
        {
            SpiritSprite.flipX = true;
        }
    }
    private void CheckPlayerDistance()
    {
        if (Mathf.Abs(playerTransform.position.x - transform.position.x) > 50f)
        {
            transform.position = spiritFollowPointTransform.position;
        }
    }
}
