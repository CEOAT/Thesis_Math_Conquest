using UnityEditor;
using UnityEngine;

namespace nightowl.distortionshaderpack
{
    public class DistortionInfoWindow : EditorWindow
    {
        // Unity
        void OnGUI()
        {
            EditorGUILayout.PrefixLabel("Hi,");
            EditorGUILayout.LabelField("Thank you for using DistortionShaderPack!");
            EditorGUILayout.LabelField("Here are some options for start working with this package.");
            EditorGUILayout.LabelField("You can always find the options at:");
            EditorGUILayout.LabelField("Window > DistortionShaderPack > ...");
            EditorGUILayout.LabelField("");
            if (GUILayout.Button("Manual"))
            {
                DistortionPackEditor.OpenManual();
            }
            if (GUILayout.Button("Demos"))
            {
                DistortionPackEditor.OpenDemos();
            }
            if (GUILayout.Button("Close"))
            {
                Close();
            }
        }
    }
}