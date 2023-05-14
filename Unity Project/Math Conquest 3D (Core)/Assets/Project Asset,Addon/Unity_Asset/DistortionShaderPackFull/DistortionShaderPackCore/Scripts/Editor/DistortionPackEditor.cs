using UnityEngine;
using UnityEditor;

namespace nightowl.distortionshaderpack
{
    [InitializeOnLoad]
    public class DistortionPackEditor : Editor
    {
        const string DATE_TIMESTAMP = "DistortionTimeStamp";
        const string COMPLETED = "DistortionFeedbackComplete";

        static DistortionPackEditor()
        {
            EditorApplication.delayCall += HandleEditorLoaded;
        }

        static void HandleEditorLoaded()
        {
            if (IsCompleted())
                return;

            if(IsFirstStart())
            {
                OpenInfoWindow();
                SetTime();
            }
            else if(IsReadyForFeedback())
            {
                OpenFeedbackWindow();
            }
        }

        static bool IsCompleted()
        {
            int value = PlayerPrefs.GetInt(COMPLETED);
            return value != 0;
        }

        static bool IsFirstStart()
        {
            string timeString = PlayerPrefs.GetString(DATE_TIMESTAMP);
            return string.IsNullOrEmpty(timeString);
        }

        static bool IsReadyForFeedback()
        {
            string timeString = PlayerPrefs.GetString(DATE_TIMESTAMP);
            if (string.IsNullOrEmpty(timeString))
                return false;

            var dateTime = System.DateTime.Parse(timeString);
            dateTime = dateTime.AddMonths(1);
            return System.DateTime.Now > dateTime;
        }

        static void Complete()
        {
            PlayerPrefs.SetInt(COMPLETED, 1);
        }
        
        static void OpenInfoWindow()
        {
            var window = EditorWindow.GetWindow(typeof(DistortionInfoWindow));
            window.title = "DistortionShaderPack Info";
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 200);
            window.Show();
        }

        [MenuItem("Window/DistortionShaderPack/FeedbackWindow")]
        static void OpenFeedbackWindow()
        {
            var window = EditorWindow.GetWindow(typeof(DistortionFeedbackWindow));
            window.title = "DistortionShaderPack Feedback";
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 200);
            window.Show();
        }

        [MenuItem("Window/DistortionShaderPack/Manual")]
        public static void OpenManual()
        {
            Application.OpenURL("https://docs.google.com/document/d/1DTisgJIXKEjpwRw6g1JgomPwWDJty7Uhvz94h4xOCBY");
        }

        static void SetTime()
        {
            PlayerPrefs.SetString(DATE_TIMESTAMP, System.DateTime.Now.ToString());
        }

        [MenuItem("Window/DistortionShaderPack/Contact (Mail)")]
        public static void SendMail()
        {
            Application.OpenURL("mailTo:mailnightowl@gmail.com");
            Complete();
        }

        [MenuItem("Window/DistortionShaderPack/Demos")]
        public static void OpenDemos()
        {
            Application.OpenURL("https://www.dropbox.com/sh/odlf41gikmzklf6/AADPsAt6mw2TXf-6RPjxdSM1a");
        }

        public static void WriteReview()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/vfx/shaders/distortion-shader-pack-62426");
            Complete();
        }

        public static void AskLaterForFeedback()
        {
            SetTime();
        }

        public static void StopAskingForFeedback()
        {
            Complete();
        }
    }

}