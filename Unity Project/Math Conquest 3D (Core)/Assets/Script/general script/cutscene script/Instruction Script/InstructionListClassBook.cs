using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstructionListClassBook
{
    public List<InstructionSet> instructionSet;
}

[System.Serializable]
public class InstructionSetBook
{
    public GameObject instructionElementObject;
}