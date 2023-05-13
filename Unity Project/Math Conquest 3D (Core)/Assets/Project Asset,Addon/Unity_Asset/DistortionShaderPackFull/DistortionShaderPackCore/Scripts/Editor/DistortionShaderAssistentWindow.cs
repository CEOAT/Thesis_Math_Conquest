using UnityEngine;
using UnityEditor;

namespace nightowl.distortionshaderpack
{
    public class DistortionShaderAssistentWindow : EditorWindow
    {
        private DistortionShaderConfig config = new DistortionShaderConfig();

        // Unity
        void OnGUI()
        {
            EditorGUILayout.PrefixLabel("Distortion shader assistent");
            config.name = EditorGUILayout.TextField("Shader name", config.name);
            EditorGUILayout.LabelField("File path: Assets/" + config.name + ".shader");
            // Render pipeline
            EditorGUI.BeginChangeCheck();
            config.UseURP = !EditorGUILayout.Toggle("Standard RenderPipeline", !config.UseURP);
            config.UseURP = EditorGUILayout.Toggle("Universal RenderPipeline", config.UseURP);
            if (EditorGUI.EndChangeCheck())
                config.CustomGrabTextureName = config.UseURP ? "_CameraOpaqueTexture" : "_GrabTexture";

            GUILayout.Label("");
            config.UseRenderTexture = EditorGUILayout.Toggle("Use RenderTexture", config.UseRenderTexture);
            config.UseCustomGrabTextureName = EditorGUILayout.Toggle("Use custom TextureName", config.UseCustomGrabTextureName);
            if (config.UseCustomGrabTextureName)
                config.CustomGrabTextureName = EditorGUILayout.TextField("Custom TextureName", config.CustomGrabTextureName);

            GUILayout.Label("");
            config.UseColor = EditorGUILayout.Toggle("Use color highlight", config.UseColor);
            config.UseVertexColor = EditorGUILayout.Toggle("Use vertex color", config.UseVertexColor);
            if (!config.UseURP)
            {
                config.UseAlpha = EditorGUILayout.Toggle("Use alpha", config.UseAlpha);
                config.UseBlending = EditorGUILayout.Toggle("Use blending", config.UseBlending);
            }
            else
            {
                config.UseAlpha = false;
                config.UseBlending = false;
            }
            config.UseCenterDistortion = EditorGUILayout.Toggle("Use center distortion", config.UseCenterDistortion);
            config.UseNormal = EditorGUILayout.Toggle("Use normal map", config.UseNormal);
            if (config.UseNormal)
                config.UseNormalMovement = EditorGUILayout.Toggle("Use normal movement", config.UseNormalMovement);

            if (GUILayout.Button("Build"))
            {
                DistortionShaderFactory.Build(config);
                //Close();
            }

            if(!config.UseNormal)
            {
                config.UseNormalMovement = false;
            }
        }
    }
}