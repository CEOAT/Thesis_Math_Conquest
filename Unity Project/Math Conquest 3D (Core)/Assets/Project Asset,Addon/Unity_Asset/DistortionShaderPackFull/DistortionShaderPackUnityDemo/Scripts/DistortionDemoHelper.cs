using UnityEngine;
using System.Collections.Generic;

namespace nightowl.distortionshaderpack.demo
{
    public class DistortionDemoHelper : MonoBehaviour
    {
        // Static
        private static List<DistortionDemoHelper> list = new List<DistortionDemoHelper>();
        private static bool isActive = true;

        public static void ToggleActive()
        {
            SetActive(!isActive);
        }

        public static void SetActive(bool active)
        {
            isActive = active;
            foreach (var demo in list)
            {
                demo.SetObjectActive(active);
            }
        }

        // Instance
        // Field
        public bool Inverted;

        // DistortionDemoHelper code
        private void Awake()
        {
            if (!list.Contains(this))
            {
                list.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (list.Contains(this))
            {
                list.Remove(this);
            }
        }

        private void SetObjectActive(bool active)
        {
            if (Inverted) active = !active;
            gameObject.SetActive(active);
        }
    }
}