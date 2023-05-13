using UnityEngine;

namespace nightowl.distortionshaderpack
{
    public class DistortionAnimator : MonoBehaviour
    {
        // Reference
        public DistortionShaderHelper Helper;

        // Fields
        public float Duration = 2;
        public AnimationCurve Scale;
        public AnimationCurve DistortionCircle;
        public AnimationCurve DistortionStrength;
        public AnimationCurve ColorStrength;
        public AnimationCurve AlphaStrength;
        public AnimationCurve BlendStrength;
        public AnimationCurve NormalDistortionStrength;
        public AnimationCurve NormalCircle;
        public AnimationCurve NormalMovementX;
        public AnimationCurve NormalMovementY;

        private float timer = 0;

        // Code
        void Start()
        {
            Reset();
        }

        void Update()
        {
            timer += Time.deltaTime;
            UpdateValues();
        }

        private void Reset()
        {
            timer = 0;
            UpdateValues();
        }

        private void UpdateValues()
        {
            if (timer > Duration)
            {
                timer = 0;
            }

            float time = timer / Duration;
            Helper.SetDistortionStrength(DistortionStrength.Evaluate(time));
            Helper.SetCircleStrength(DistortionCircle.Evaluate(time));
            Helper.SetAlpha(AlphaStrength.Evaluate(time));
            Helper.SetBlend(BlendStrength.Evaluate(time));
            Helper.SetNormalCircleStrength(NormalCircle.Evaluate(time));
            Helper.SetNormalStrength(NormalDistortionStrength.Evaluate(time));
            Helper.SetNormalMovement(new Vector2(NormalMovementX.Evaluate(time), NormalMovementY.Evaluate(time)));
            Helper.SetColorStrength(ColorStrength.Evaluate(time));
            transform.localScale = Vector3.one * Scale.Evaluate(time);
        }
    }
}