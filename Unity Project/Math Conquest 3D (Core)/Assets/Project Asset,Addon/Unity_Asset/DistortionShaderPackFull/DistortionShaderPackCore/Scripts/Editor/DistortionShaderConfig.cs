namespace nightowl.distortionshaderpack
{
    public class DistortionShaderConfig
    {
        // Properties
        public bool UsesColors { get { return UseColor || UseVertexColor; } }

        // Fields
        public string name = "Custom";
        public bool UseRenderTexture = false;
        public bool UseCustomGrabTextureName = false;
        public string CustomGrabTextureName = "_GrabTexture";
        public bool UseURP = false;
        public bool UseColor = true;
        public bool UseVertexColor = true;
        public bool UseAlpha = false;
        public bool UseCenterDistortion = true;
        public bool UseBlending = false;
        public bool UseNormal = true;
        public bool UseNormalMovement = true;

        public override string ToString()
        {
            return "Shader name: " + name + "\n" +
                "RenderTexture: " + UseRenderTexture + "\n" +
                "CustomGrabTextureName: " + UseCustomGrabTextureName + "\n" +
                "UniversalRenderPipeline: " + UseURP + "\n" +
                "Color highlight: " + UseColor + "\n" +
                "Vertex Color: " + UseVertexColor + "\n" +
                "Aplha: " + UseAlpha + "\n" +
                "Blending: " + UseBlending + "\n" +
                "Center distortion: " + UseCenterDistortion + "\n" +
                "Normal: " + UseNormal + "\n" +
                "Normal movement: " + UseNormalMovement;
        }
    }
}