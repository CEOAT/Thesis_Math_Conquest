using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstructionListClass
{
    public List<InstructionSet> instructionSet;
}

[System.Serializable]
public class InstructionSet
{
    public List<InstructionData> instructionData;
}

[System.Serializable]
public class InstructionData
{
    public GameObject instructionElementObject;
    public AnimationClip instructionElementAnimation;
}