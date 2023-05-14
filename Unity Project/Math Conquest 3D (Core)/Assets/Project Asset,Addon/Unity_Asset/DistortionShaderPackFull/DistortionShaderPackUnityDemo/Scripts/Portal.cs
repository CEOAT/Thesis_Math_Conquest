using UnityEngine;

namespace nightowl.distortionshaderpack.demo
{
	public class Portal : MonoBehaviour
	{
		// Refs
		public Material Material;
		public Light Light;
		public Color PortalColor = Color.magenta;
		public float TargetDistortionStrength = -2;
		public float TargetNormalDistortion = 1;
		public float Speed = 0.3f;
		public float TimeOffset = 0;
        public Rigidbody SpawnPrefab;

        private float lastFrac;
        private Rigidbody spawnObject;

        // Mono
        private void Start()
        {
            Light.color = PortalColor;
        }

        private void Update()
		{
			float frac = (Mathf.Sin(Time.time + TimeOffset)*0.5f + 0.5f)/Speed;


			Material.SetFloat("_DistortionStrength", frac*TargetDistortionStrength);
			Material.SetFloat("_NormalTexStrength", frac*TargetNormalDistortion);
			Material.SetColor("_MainColor", frac*PortalColor);

			Light.intensity = frac;

            if(lastFrac > frac && spawnObject == null)
            {
                Spawn();
            }
            else if(lastFrac < frac)
            {
                if (spawnObject != null)
                {
                    Destroy(spawnObject.gameObject);
                }
            }
            lastFrac = frac;
		}

		// Portal
		private void Spawn()
		{
            if (SpawnPrefab == null)
                return;

            spawnObject = Instantiate(SpawnPrefab);
            spawnObject.transform.position = transform.position + transform.forward* spawnObject.transform.localScale.x;
            spawnObject.velocity = -transform.forward*3;
        }
	}
}