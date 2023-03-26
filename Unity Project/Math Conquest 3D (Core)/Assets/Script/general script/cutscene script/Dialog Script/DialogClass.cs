using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogClass
{
    public List<DialogData> DialogSet;
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