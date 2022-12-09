using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeObjectInteractableNode : MonoBehaviour
{
    [SerializeField] private ExplorationModeObjectInteractableNodeController NodeController;

    private void Start()
    {
        ActiveNode();
    }
    private void ActiveNode()
    {
        NodeController.ActiveNode();
    }
}