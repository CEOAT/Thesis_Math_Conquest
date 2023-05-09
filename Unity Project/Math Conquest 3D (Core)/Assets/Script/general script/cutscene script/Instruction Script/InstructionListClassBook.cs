using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstructionListClassBook
{
    public List<InstructionSetBook> instructionSet;
}

[System.Serializable]
public class InstructionSetBook
{
    public List<GameObject> instructionElementObjectList;
}