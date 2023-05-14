using UnityEngine;
using UnityEngine.SceneManagement;

namespace nightowl.distortionshaderpack.demo
{
    public class DistortionDemoInput : MonoBehaviour
    {
        private void Start()
        {
            DistortionDemoHelper.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DistortionDemoHelper.ToggleActive();
            }
            if (Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}