using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class USBReplacementController : MonoBehaviour
{
    public Shader m_replacementShader;

    void OnEnable()
    {
        if (m_replacementShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(m_replacementShader,"RenderType");
            Debug.Log("PrintOnEnable: script was enabled");
        }
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
        Debug.Log("PrintOnDisable: script was disabled");
    }
}
