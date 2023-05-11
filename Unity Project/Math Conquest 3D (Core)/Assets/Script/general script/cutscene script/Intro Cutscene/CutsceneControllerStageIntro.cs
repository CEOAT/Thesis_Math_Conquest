using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneControllerStageIntro : MonoBehaviour
{
    [SerializeField] private GameObject transitionCanvasPrefab;
    private bool isSkipSequenceStart;

    private MasterInput PlayerInput;
    private CinemachineBrain CinemachineBrain;
    private CinemachineVirtualCamera VirtualCamera;
    
    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        CinemachineBrain = GetComponent<CinemachineBrain>();
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void SetupControl()
    {
        PlayerInput = new MasterInput();
        PlayerInput.PlayerControlGeneral.SkipCutscene.performed += context => SkipStageIntorCutscene();
        PlayerInput.Enable();
    }

    private void SkipStageIntorCutscene()
    {
        if(isSkipSequenceStart == false)
        {
            isSkipSequenceStart = true;
            StartCoroutine(SkipSequence());
        }
    }
    private IEnumerator SkipSequence()
    {
        Destroy(Instantiate(transitionCanvasPrefab),3f);
        yield return new WaitForSeconds(0.4f);
        VirtualCamera.m_Priority = 5000;
        yield return new WaitForSeconds(0.05f);
        CinemachineBrain.m_DefaultBlend.m_Time = 0f;
        VirtualCamera.m_Priority = -5000;
    }

    private void OnDisable()
    {
        RemoveControl();
    }
    private void RemoveControl()
    {
        PlayerInput.PlayerControlGeneral.SkipCutscene.performed -= context => SkipStageIntorCutscene();
        PlayerInput.Disable();
    }
}
