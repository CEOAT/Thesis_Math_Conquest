using UnityEngine;

namespace nightowl.distortionshaderpack
{
    public class Rotate : MonoBehaviour
	{
		// Refs
		public Vector3 RotateAxis = Vector3.up;
		public float speed = 1;

		// Mono
		void Update()
		{
			transform.Rotate(RotateAxis, speed);
		}
	}
}