using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    [SerializeField] private Vector3 Force;

    private Rigidbody thisRigid;
    // Start is called before the first frame update
    void Start()
    {
        thisRigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        thisRigid.AddForce(Force);
    }
}
