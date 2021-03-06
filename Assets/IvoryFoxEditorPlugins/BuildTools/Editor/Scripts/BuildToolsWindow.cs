using System;
using System.IO;
using System.Linq;
using ReportTools.Editor.Scripts;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BuildTools.Editor.Scripts
{
    public class BuildToolsWindow : EditorWindow
    {
        private BuildData _commonBuildData;
        private BuildData _releaseBuildData;
        private BuildData _debugBuildData;
        private static BuildToolsSettings _settings;

        private EnumField _screenOrientationField;
        private TextField _taskNumberField;
        private TextField _appVersionField;

        private string buildsFolderPath;
    
        [MenuItem("IvoryFox/Build Tools")]
        public static void ShowWindow() => GetWindow<BuildToolsWindow>("Build Tools");

        private void Awake()
        {
            buildsFolderPath = Application.dataPath.Replace("/Assets", "") + "/Builds";
        }

        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("BuildToolsUI");
            visualTree.CloneTree(rootVisualElement);
            
            Connect();
            OnSelectionChange();
        }

        private void Connect()
        {
            
            _settings = Resources.Load<BuildToolsSettings>("BuildToolsSettings");
            
            _commonBuildData = Resources.Load<BuildData>("BuildData_Common") ?? LoadData("BuildData_Common");
            _debugBuildData = Resources.Load<BuildData>("BuildData_Debug") ?? LoadData("BuildData_Debug");
            _releaseBuildData = Resources.Load<BuildData>("BuildData_Release") ?? LoadData("BuildData_Release");

            var buildToolsVersionLabel = rootVisualElement.Q<Label>("BuildToolsVersionLabel");
            buildToolsVersionLabel.text = _settings.version;

            /*var buildPathTextField = rootVisualElement.Q<TextField>("BuildsFolderPathTextField");
            buildPathTextField.value = _settings.buildFolderPath;
            buildPathTextField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            buildPathTextField.RegisterCallback<InputEvent>((evt) =>
            {
                _settings.buildFolderPath = buildPathTextField.value;
            });
            
            var buildSelectFolderButton = rootVisualElement.Q<Button>("SelectBuildsFolderButton");
            buildSelectFolderButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                buildPathTextField.value = GetFolderPath();
                _settings.buildFolderPath = buildPathTextField.value;
            });*/
            
            _screenOrientationField = rootVisualElement.Q<EnumField>("ScreenOrientationEnum");
            _screenOrientationField.Init(_commonBuildData.screenOrientation);
            _screenOrientationField.value = _commonBuildData.screenOrientation;
            _screenOrientationField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            _screenOrientationField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
            {
                Debug.Log($"Changed Screen Orientation | from {_commonBuildData.screenOrientation} to {(UIOrientation) evt.newValue}");
                _commonBuildData.screenOrientation = (UIOrientation) evt.newValue;
            });

            _taskNumberField = rootVisualElement.Q<TextField>("TaskNumberField");
            _taskNumberField.value = _commonBuildData.taskNumber;
            _taskNumberField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            _taskNumberField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed Task Number | from {_commonBuildData.taskNumber} to {evt.newData}");
                _commonBuildData.taskNumber = evt.newData;
            });
            
            if (string.IsNullOrEmpty(_commonBuildData.taskNumber))
            {
                _taskNumberField.value = SetTaskNumber();
                _commonBuildData.taskNumber = _taskNumberField.value;
            }
            
            var autoSetTaskNumberToggle = rootVisualElement.Q<Button>("SetTaskNumberButton");
            autoSetTaskNumberToggle.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            autoSetTaskNumberToggle.RegisterCallback<MouseUpEvent>((evt) =>
            {
                var newTaskNumber = SetTaskNumber();
                Debug.Log($"Changed Task Number from Button | from {_commonBuildData.taskNumber} to {newTaskNumber}");
                _commonBuildData.taskNumber = newTaskNumber;
                _taskNumberField.value = newTaskNumber;
            });

            _appVersionField = rootVisualElement.Q<TextField>("AppVersionField");
            _appVersionField.value = _commonBuildData.productVersion;
            _appVersionField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            _appVersionField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed App Version | from {_commonBuildData.productVersion} to {evt.newData}");
                _commonBuildData.productVersion = evt.newData;
            });
            
            ConnectSpecificData(_debugBuildData, "Debug");
            ConnectSpecificData(_releaseBuildData, "Release");
        }

        private BuildData LoadData(string assetName)
        {
            string packageManagerFolder = Application.dataPath + "/PackageManagerAssets/Resources";
            string originalDataPath = $"Packages/com.ivoryfox.build-tools/Editor/Resources/Samples/{assetName}.asset";
            //string originalDataPath = $"Assets/BuildTools/Editor/Resources/Samples/{assetName}.asset";
            string savePath = $"Assets/PackageManagerAssets/Resources/{assetName}.asset";

            if (!Directory.Exists(packageManagerFolder)) Directory.CreateDirectory(packageManagerFolder);
            
            AssetDatabase.CopyAsset(originalDataPath, savePath);
            AssetDatabase.SaveAssets();
                
            return Resources.Load<BuildData>(assetName);
        }

        private void ConnectSpecificData(BuildData data, string key)
        {
            var appNameField = rootVisualElement.Q<TextField>(key+ "AppName");
            appNameField.value = data.productName;
            appNameField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            appNameField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed App Name | from {data.productName} to {evt.newData}");
                data.productName = evt.newData;
            });
            
            var packageIdField = rootVisualElement.Q<TextField>(key+ "PackageId");
            packageIdField.value = data.packageId;
            packageIdField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
            packageIdField.RegisterCallback<InputEvent>((evt) =>
            {
                Debug.Log($"Changed Package Id | from {data.packageId} to {evt.newData}");
                data.packageId = evt.newData;
                data.companyName = SetCompanyName(evt.newData);
            });
            
            var iconField = rootVisualElement.Q<ObjectField>(key + "Icon");
            iconField.objectType = typeof(Texture2D);
            iconField.value = data.icon;
            iconField.RegisterCallback<FocusOutEvent>(evt => SaveDataChanges());
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
                if (SetPlayerData(data)) Report.CreateReport($"{buildsFolderPath}/{data.GetApkName()}");
            });
            
            var buildButton = rootVisualElement.Q<Button>(key + "BuildButton");
            buildButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                if (SetPlayerData(data)) ApkBuilder.BuildApk(data, buildsFolderPath);
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
            EditorUtility.SetDirty(_settings);
            EditorUtility.SetDirty(_commonBuildData);
            EditorUtility.SetDirty(_debugBuildData);
            EditorUtility.SetDirty(_releaseBuildData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private string GetFolderPath() => EditorUtility.OpenFolderPanel("", "", "");
        private string GetFilePath() => EditorUtility.OpenFilePanel("","","");

        private bool SetPlayerData(BuildData data)
        {
            data.productVersion = _commonBuildData.productVersion;
            data.screenOrientation = _commonBuildData.screenOrientation;
            data.taskNumber = _commonBuildData.taskNumber;
            
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

            if (digits.Length == 0) return _commonBuildData.taskNumber;
            else return digits;
        }
    }
}