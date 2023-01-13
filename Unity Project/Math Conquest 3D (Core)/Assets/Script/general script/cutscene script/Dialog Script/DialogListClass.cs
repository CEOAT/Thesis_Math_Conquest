using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogListClass
{
    public List<DialogSet> DialogSet;
}

[System.Serializable]
public class DialogSet
{
    public List<DialogData> DialogData;
}

[System.Serializable]
public class DialogData
{
    public string speakerString;
    public Sprite speakerSprite;
    public GameObject speakerEmotionEffect;
    public Sprite backgroundSprite;
    [TextArea(3, 10)] public string dialogString;
}