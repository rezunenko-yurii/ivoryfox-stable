using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BuildTools.Editor.Scripts
{
    public class ReportToolWindow : EditorWindow
    {
        
        private static ReportToolSettings settings;
        
        [MenuItem("IvoryFox/Report Tool")]
        public static void ShowWindow() => GetWindow<ReportToolWindow>("Report Tool");
        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("ReportToolUI");
            visualTree.CloneTree(rootVisualElement);
            Connect();
            OnSelectionChange();
        }
        
        //private string fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private void Connect()
        {
            settings = Resources.Load<ReportToolSettings>("ReportToolSettings");
            
            /*var reportToolVersionLabel = rootVisualElement.Q<Label>("ReportToolVersionLabel");
            reportToolVersionLabel.text = settings.version;*/
            
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
            
            var apkSignerPathTextField = rootVisualElement.Q<TextField>("ApkSignerPathTextField");
            apkSignerPathTextField.value = settings.apkSignerPath;
            apkSignerPathTextField.RegisterCallback<InputEvent>((evt) =>
            {
                settings.apkSignerPath = apkSignerPathTextField.value;
            });
            
            var apkSignerSelectFolderButton = rootVisualElement.Q<Button>("SelectSignerFolderButton");
            apkSignerSelectFolderButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                apkSignerPathTextField.value = GetFolderPath();
                settings.apkSignerPath = apkSignerPathTextField.value;
            });
            
            var createReportButton = rootVisualElement.Q<Button>("CreateReportButton");
            createReportButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                Report.CreateReport(apkPathField.value, settings.apkSignerPath);
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