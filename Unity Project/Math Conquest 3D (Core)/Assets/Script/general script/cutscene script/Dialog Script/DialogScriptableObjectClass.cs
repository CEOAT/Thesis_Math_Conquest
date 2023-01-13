using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Assets/Create Dialog Object")]
public class DialogScriptableObjectClass : ScriptableObject
{
    public List<DialogListClass> dialogList;
}