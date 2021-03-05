using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BuildTools.Editor.Scripts
{
    public class BuildToolsWindow : EditorWindow
    {
        private BuildData commonBuildData;
        private BuildData releaseBuildData;
        private BuildData debugBuildData;
        private static BuildToolsSettings settings;

        private EnumField screenOrientationField;
        private TextField taskNumberField;
        private TextField appVersionField;
    
        [MenuItem("IvoryFox/Build Tools")]
        public static void ShowWindow() => GetWindow<BuildToolsWindow>("Build Tools");

        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("BuildToolsUI");
            visualTree.CloneTree(rootVisualElement);
            Connect();
            OnSelectionChange();
        }

        private void Connect()
        {
            settings = Resources.Load<BuildToolsSettings>("BuildToolsSettings");
            commonBuildData = Resources.Load<BuildData>("BuildData_Common");
            debugBuildData = Resources.Load<BuildData>("BuildData_Debug");
            releaseBuildData = Resources.Load<BuildData>("BuildData_Release");
            
            var buildToolsVersionLabel = rootVisualElement.Q<Label>("BuildToolsVersionLabel");
            buildToolsVersionLabel.text = settings.version;

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
            
            var buildPathTextField = rootVisualElement.Q<TextField>("BuildsFolderPathTextField");
            buildPathTextField.value = settings.buildFolderPath;
            buildPathTextField.RegisterCallback<InputEvent>((evt) =>
            {
                settings.buildFolderPath = buildPathTextField.value;
            });
            
            var buildSelectFolderButton = rootVisualElement.Q<Button>("SelectBuildsFolderButton");
            buildSelectFolderButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                buildPathTextField.value = GetFolderPath();
                settings.buildFolderPath = buildPathTextField.value;
            });
            
            screenOrientationField = rootVisualElement.Q<EnumField>("ScreenOrientationEnum");
            screenOrientationField.Init(commonBuildData.screenOrientation);
            screenOrientationField.value = commonBuildData.screenOrientation;
            screenOrientationField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
            {
                Debug.Log($"Changed Screen Orientation | from {commonBuildData.screenOrientation} to {(UIOrientation) evt.newValue}");
                commonBuildData.screenOrientation = (UIOrientation) evt.newValue;
            });

            taskNumberField = rootVisualElement.Q<TextField>("TaskNumberField");
            taskNumberField.value = commonBuildData.taskNumber;
            taskNumberField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed Task Number | from {commonBuildData.taskNumber} to {evt.newData}");
                commonBuildData.taskNumber = evt.newData;
            });
            
            if (string.IsNullOrEmpty(commonBuildData.taskNumber))
            {
                taskNumberField.value = SetTaskNumber();
                commonBuildData.taskNumber = taskNumberField.value;
            }
            
            var autoSetTaskNumberToggle = rootVisualElement.Q<Button>("SetTaskNumberButton");
            autoSetTaskNumberToggle.RegisterCallback<MouseUpEvent>((evt) =>
            {
                var newTaskNumber = SetTaskNumber();
                Debug.Log($"Changed Task Number from Button | from {commonBuildData.taskNumber} to {newTaskNumber}");
                commonBuildData.taskNumber = newTaskNumber;
                taskNumberField.value = newTaskNumber;
            });

            appVersionField = rootVisualElement.Q<TextField>("AppVersionField");
            appVersionField.value = commonBuildData.productVersion;
            appVersionField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed App Version | from {commonBuildData.productVersion} to {evt.newData}");
                commonBuildData.productVersion = evt.newData;
            });
            
            var uxmlButton = rootVisualElement.Q<Button>("SaveAllDataButton");
            uxmlButton.RegisterCallback<MouseUpEvent>((evt) => SaveDataChanges());

            ConnectSpecificData(debugBuildData, "Debug");
            ConnectSpecificData(releaseBuildData, "Release");
        }

        private void ConnectSpecificData(BuildData data, string key)
        {
            var appNameField = rootVisualElement.Q<TextField>(key+ "AppName");
            appNameField.value = data.productName;
            appNameField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed App Name | from {data.productName} to {evt.newData}");
                data.productName = evt.newData;
            });
            
            var packageIdField = rootVisualElement.Q<TextField>(key+ "PackageId");
            packageIdField.value = data.packageId;
            packageIdField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed Package Id | from {data.packageId} to {evt.newData}");
                data.packageId = evt.newData;
                data.companyName = SetCompanyName(evt.newData);
            });
            
            var iconField = rootVisualElement.Q<ObjectField>(key + "Icon");
            iconField.objectType = typeof(Texture2D);
            iconField.value = data.icon;
            iconField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                data.icon = (Texture2D) evt.newValue;
            });

            var setDataButton = rootVisualElement.Q<Button>(key + "SetDataButton");
            setDataButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                SetPlayerData(data);
            });
            
            var reportButton = rootVisualElement.Q<Button>(key + "ReportButton");
            reportButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                if (SetPlayerData(data)) Report.CreateReport($"{settings.buildFolderPath}/{data.GetApkName}", settings.apkSignerPath);
            });
            
            var buildButton = rootVisualElement.Q<Button>(key + "BuildButton");
            buildButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                if (SetPlayerData(data)) ApkBuilder.BuildApk(data, settings.buildFolderPath);
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

        private void SaveDataChanges()
        {
            EditorUtility.SetDirty(settings);
            EditorUtility.SetDirty(commonBuildData);
            EditorUtility.SetDirty(debugBuildData);
            EditorUtility.SetDirty(releaseBuildData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private string GetFolderPath() => EditorUtility.OpenFolderPanel("", "", "");
        private string GetFilePath() => EditorUtility.OpenFilePanel("","","");

        private bool SetPlayerData(BuildData data)
        {
            data.productVersion = commonBuildData.productVersion;
            data.screenOrientation = commonBuildData.screenOrientation;
            data.taskNumber = commonBuildData.taskNumber;
            
            if (!data.IsAllDataInput())
            {
                Debug.LogError("Input all data first");
                return false;
            }

            PlayerHelper.SetPlayerSettings(data);
            return true;
        }

        private string SetCompanyName(string packageId)
        {
            if (!string.IsNullOrEmpty(packageId))
            {
                int firstDotInd = packageId.IndexOf(".") + 1;
                int lastDotInd = packageId.LastIndexOf(".");

                if (firstDotInd > 0 && lastDotInd > 0 && firstDotInd != lastDotInd + 1)
                {
                    string beforedot = packageId.Substring(firstDotInd, lastDotInd - firstDotInd);
                    return beforedot;
                }
            }

            return string.Empty;
        }

        private string SetTaskNumber()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            string digits = String.Join("", projectName.Where(char.IsDigit));

            if (digits.Length == 0) return commonBuildData.taskNumber;
            else return digits;
        }

        /*public int callbackOrder { get; }
        public void OnPreprocessBuild(BuildReport report) => PipelineBuilder.PreprocessBuildCheck();*/
    }
}