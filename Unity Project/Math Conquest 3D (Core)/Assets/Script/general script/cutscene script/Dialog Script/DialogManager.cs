using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager: MonoBehaviour
{
    [SerializeField] public GameObject inGameCanvas;
    [SerializeField] public GameObject DialogUI;
    [SerializeField] public Image backgroundImage;
    [SerializeField] public Image speakerImage;
    [SerializeField] public TMP_Text speakerText;
    [SerializeField] public Transform speakerEffectSpawnpoint;
    [SerializeField] public TMP_Text dialogText;
    [SerializeField] public Animator dialogButton;
    [SerializeField] public GameObject backgroundTransitionPrefab; 
}