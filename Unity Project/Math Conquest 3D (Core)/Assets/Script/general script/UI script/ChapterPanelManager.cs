using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanelManager : MonoBehaviour
{
    [Serializable]
    public struct Chapters
    {
        public string Eventname;
        public Button button;
        public MainMenuManager.ButtonBehavior behavior;
        public GameObject From;
        public GameObject To;
    }

    [SerializeField] private Chapters ButtonEvents;
    
}
