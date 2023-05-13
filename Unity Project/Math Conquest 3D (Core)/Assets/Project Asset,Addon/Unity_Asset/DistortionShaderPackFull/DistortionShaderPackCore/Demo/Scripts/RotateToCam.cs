using UnityEngine;

namespace nightowl.distortionshaderpack
{
    [ExecuteInEditMode]
    public class RotateToCam : MonoBehaviour
    {
        // References
        public Camera targetCamera;
        public bool Inverted = true;
        public bool rotateX = true;
        public bool rotateY = true;
        public bool rotateZ = true;
        public bool useUpdate = false;

        // Code
        public void OnWillRenderObject()
        {
            if (useUpdate)
                return;

            UpdateRotation();
        }

        public void Update()
        {
            if (!useUpdate)
                return;

            UpdateRotation();
        }

        private void UpdateRotation()
        {
            Camera cam = GetCamera();
            if (cam == null)
                return;

            Vector3 direction;
            if (Inverted)
                direction = transform.position - cam.transform.position;
            else
                direction = cam.transform.position - transform.position;

            direction = LockAxes(direction);
            transform.LookAt(transform.position + direction);
        }

        private Camera GetCamera()
        {
            Camera cam = targetCamera;
            if (cam == null)
            {
                cam = Camera.current;
            }
            return cam;
        }

        private Vector3 LockAxes(Vector3 direction)
        {
            if (!rotateX)
                direction.x = 0;
            if (!rotateY)
                direction.y = 0;
            if (!rotateZ)
                direction.z = 0;

            return direction;
        }
    }
}