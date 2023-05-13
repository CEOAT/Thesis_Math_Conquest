using UnityEngine;

[ExecuteInEditMode]
public class RenderTextureHelper : MonoBehaviour
{
    // Reference
    public Camera MainCamera;
    public Camera CamCopy;
    public Material[] Materials;

    // Fields
    public string GrabTextureName = "_GrabTexture";
    public LayerMask CullingMask;

    // RenderTextureHelper
    void Start()
    {
        if(CamCopy == null && MainCamera != null)
        {
            SetCamera(MainCamera, true);
        }
    }
    
    void Update()
    {
        if (CamCopy == null || Materials.Length == 0)
            return;

        CamCopy.Render();
    }

    public void SetCamera(Camera mainCamera, bool createCopy = false)
    {
        MainCamera = mainCamera;

        if (createCopy)
        {
            if (CamCopy != null)
            {
                Destroy(CamCopy.gameObject);
            }

            CamCopy = new GameObject().AddComponent<Camera>();
            CamCopy.gameObject.name = "camCopy";
            CamCopy.CopyFrom(MainCamera);
            CamCopy.gameObject.hideFlags = HideFlags.DontSave;
            CamCopy.cullingMask = CullingMask;
            CamCopy.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            CamCopy.enabled = false;
            CamCopy.transform.SetParent(MainCamera.transform);
            CamCopy.transform.localPosition = Vector3.zero;
            CamCopy.transform.localRotation = Quaternion.identity;
            DestroyImmediate(CamCopy.GetComponent<RenderTextureHelper>());
        }
        UpdateRenderTexture();
    }

    public void SetCopyCamera(Camera copyCamera)
    {

        if (CamCopy != null && CamCopy != copyCamera)
        {
            Destroy(CamCopy.gameObject);
        }
        CamCopy = copyCamera;
        UpdateRenderTexture();
    }

    public void UpdateRenderTexture()
    {
        if (MainCamera != null && Materials.Length > 0)
        {
            foreach (Material mat in Materials)
            {
                mat.SetTexture(GrabTextureName, CamCopy.targetTexture);
            }
        }
    }
}
