using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Animator))]

public class ExplorationModeObjectTrap : MonoBehaviour
{
    // control the trap holding this script

    [Tooltip("Type of trap")] public TrapType trapType;
    public enum TrapType
    {
        idle,       //fall dead check, lava
        rotate,     //rotate around it self
        inOut,      //spike
        pendulum    //move like pendulu,
    };
    public bool isTrapTrigger;
    public bool isDealDamage;
    [SerializeField] public float trapSpeed;
    [SerializeField] public float trapDamage;

    private Animator animator;
    private Collider collider;

    private void Start()
    {
        SetupComponent();
        SetupTrapType();
        SetupTrapSpeed();
        SetupTrapCollider();
    }
    private void SetupComponent()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }
    private void SetupTrapType()
    {
        switch (trapType)
        {
            case TrapType.idle:
                {
                    break;
                }
            case TrapType.rotate:
                {
                    animator.SetTrigger("trigger_trap_rotate");
                    break;
                }
            case TrapType.inOut:
                {
                    animator.SetTrigger("trigger_trap_in-out");
                    break;
                }
            case TrapType.pendulum:
                {
                    animator.SetTrigger("trigger_trap_pendulum");
                    break;
                }
        }
    }
    private void SetupTrapSpeed()
    {
        if (trapSpeed <= 0) // trap speed cannot be 0 or below
        {
            trapSpeed = 1f;
        }
        if (trapType == TrapType.idle)  // when trap is idle then speed is 0
        {
            trapSpeed = 0f;
        }

        animator.speed = trapSpeed;
    }
    private void SetupTrapCollider()
    {
        if (isTrapTrigger == true)
        {
            collider.isTrigger = true;
        }
        else if (isTrapTrigger == false)
        {
            collider.isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(trapDamage);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(trapDamage);
        }
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(ExplorationModeObjectTrap))]
    public class ObjectTrapTester : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ExplorationModeObjectTrap trap = (ExplorationModeObjectTrap)target;

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Need to field value", EditorStyles.boldLabel);
            
            if (trap.trapType != TrapType.idle)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField("Trap Speed", GUILayout.MaxWidth(100));
                trap.trapSpeed = EditorGUILayout.FloatField(trap.trapSpeed);
                EditorGUILayout.EndHorizontal();
            }

            if (trap.isDealDamage)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField("Trap Damage", GUILayout.MaxWidth(100));
                trap.trapDamage = EditorGUILayout.FloatField(trap.trapDamage);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
    #endregion Editor
}
