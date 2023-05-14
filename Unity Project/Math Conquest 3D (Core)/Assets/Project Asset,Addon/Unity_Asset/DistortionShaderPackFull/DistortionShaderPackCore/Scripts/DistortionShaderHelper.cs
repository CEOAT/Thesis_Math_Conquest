using UnityEngine;

namespace nightowl.distortionshaderpack
{
    public class DistortionShaderHelper : MonoBehaviour
    {
        // Reference
        public Material Material;

        // Code
        public void SetMaterial(Material aMaterial)
        {
            Material = aMaterial;
        }

        public void SetColor(Color aColor)
        {
            SetColor("_MainColor", aColor);
        }

        public void SetColorStrength(float strength)
        {
            SetFloat("_StrengthColor", strength);
        }

        public void SetDistortionStrength(float strength)
        {
            SetFloat("_DistortionStrength", strength);
        }

        public void SetCircleStrength(float strength)
        {
            SetFloat("_DistortionCircle", strength);
        }

        public void SetGrabTexture(string textureName, Texture aTexture)
        {
            SetTexture(textureName, aTexture);
        }

        public void SetNormalMap(Texture aTexture)
        {
            SetTexture("_NormalTexture", aTexture);
        }

        public void SetNormalScale(Vector2 scale)
        {
            SetTextureScale("_NormalTexture", scale);
        }

        public void SetNormalOffset(Vector2 offset)
        {
            SetTextureOffset("_NormalTexture", offset);
        }

        public void SetNormalStrength(float strength)
        {
            SetFloat("_NormalTexStrength", strength);
        }

        public void SetNormalCircleStrength(float strength)
        {
            SetFloat("_NormalTexFrameless", strength);
        }

        public void SetNormalMovement(Vector2 movement)
        {
            SetVector("_UVOffset", movement);
        }

        public void SetAlpha(float strength)
        {
            SetFloat("_StrengthAlpha", strength);
        }

        public void SetBlend(float strength)
        {
            SetFloat("_StrengthBlend", strength);
        }

        private bool IsValid(string key)
        {
            if (Material == null)
                return false;

            return Material.HasProperty(key);
        }

        private void SetColor(string key, Color value)
        {
            if (!IsValid(key))
                return;

            Material.SetColor(key, value);
        }

        private void SetFloat(string key, float value)
        {
            if (!IsValid(key))
                return;

            Material.SetFloat(key, value);
        }

        private void SetVector(string key, Vector4 value)
        {
            if (!IsValid(key))
                return;

            Material.SetVector(key, value);
        }

        private void SetTexture(string key, Texture value)
        {
            if (!IsValid(key))
                return;

            Material.SetTexture(key, value);
        }

        private void SetTextureScale(string key, Vector2 value)
        {
            if (!IsValid(key))
                return;

            Material.SetTextureScale(key, value);
        }

        private void SetTextureOffset(string key, Vector2 value)
        {
            if (!IsValid(key))
                return;

            Material.SetTextureOffset(key, value);
        }
    }
}