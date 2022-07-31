using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonModeFallCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = new Vector3(-58.9f,0f,0f);
        }
    }
}
