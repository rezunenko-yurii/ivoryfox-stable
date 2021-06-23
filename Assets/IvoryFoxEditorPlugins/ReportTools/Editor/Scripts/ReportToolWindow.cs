using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReportTools.Editor.Scripts
{
    public class ReportToolWindow : EditorWindow
    {
        
        private static ReportToolSettings _settings;
        
        [MenuItem("IvoryFox/Report Tool")]
        public static void ShowWindow() => GetWindow<ReportToolWindow>("Report Tool");
        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("ReportToolUI");
            visualTree.CloneTree(rootVisualElement);
            Connect();
            OnSelectionChange();
        }
        private void Connect()
        {
            _settings = Resources.Load<ReportToolSettings>("ReportToolSettings");
            
            var apkPathField = rootVisualElement.Q<TextField>("ApkPathField");
            apkPathField.RegisterCallback<InputEvent>((evt) =>
            {
                apkPathField.value = evt.newData;
            });
            
            var chooseApkButton = rootVisualElement.Q<Button>("ChooseApkButton");
            chooseApkButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                apkPathField.value = GetFilePath();
            });

            var createReportButton = rootVisualElement.Q<Button>("CreateReportButton");
            createReportButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                Report.CreateReport(apkPathField.value);
            });
        }
        
        public void OnSelectionChange()
        {
            GameObject selectedObject = Selection.activeObject as GameObject;
            if (selectedObject != null)
            {
                SerializedObject so = new SerializedObject(selectedObject);
                rootVisualElement.Bind(so);
            }
            else rootVisualElement.Unbind();
        }
        
        private string GetFilePath() => EditorUtility.OpenFilePanel("","","");
        private string GetFolderPath() => EditorUtility.OpenFolderPanel("", "", "");
    }
}