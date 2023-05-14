using UnityEditor;
using UnityEngine;

namespace nightowl.distortionshaderpack
{
    public class DistortionFeedbackWindow : EditorWindow
    {
        // Unity
        void OnGUI()
        {
            EditorGUILayout.PrefixLabel("Feedback");
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Thank you for using DistortionShaderPack!");
            EditorGUILayout.LabelField("As developer I am interested in improving the package.");
            EditorGUILayout.LabelField("Please send me a mail with feedback or write a review.");
            EditorGUILayout.LabelField("");
            if (GUILayout.Button("Feedback mail"))
            {
                DistortionPackEditor.SendMail();
            }
            if (GUILayout.Button("Review"))
            {
                DistortionPackEditor.WriteReview();
            }
            if (GUILayout.Button("Later"))
            {
                DistortionPackEditor.AskLaterForFeedback();
                Close();
            }
            if (GUILayout.Button("Stop asking"))
            {
                DistortionPackEditor.StopAskingForFeedback();
                Close();
            }
        }
    }
}