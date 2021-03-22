using CloudContentDelivery.Runtime.Scripts;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CloudContentDelivery.Editor.Scripts
{
    public class CcdWindowElements
    {
        private readonly VisualElement _rootVisualElement;
        private BucketData _bucketData;
        private readonly CloudContentSettings _settings;
        
        public CcdWindowElements(VisualElement rootVisualElement, CloudContentSettings settings,BucketData bucketData)
        {
            _rootVisualElement = rootVisualElement;
            _settings = settings;
            _bucketData = bucketData;
            
            Connect();
        }
        public Label LogLabel { get; private set; }
        public TextField ApiKeyTextField { get; private set; }
        public TextField ContentFolderTextField { get; private set; }
        public Button SelectFolderButton { get; private set; }
        public ObjectField BucketObjectField { get; private set; }
        public TextField BucketNameTextField { get; private set; }
        public TextField BucketIdTextField { get; private set; }
        public TextField LastReleaseIdTextField { get; private set; }
        public Button CheckBucketButton { get; private set; }
        public Button ShowFilesInBucketButton { get; private set; }
        public Button SynsContentButton { get; private set; }
        public Button SaveChangesButton { get; private set; }
        public Button ClearLogButton { get; private set; }
        public Button CreateBucketButton { get; private set; }
        public Button DeleteContentButton { get; private set; }
        public Button DeleteBucketButton { get; private set; }
        public Button CreateReleaseButton { get; private set; }
        public Button LoadLastReleaseButton { get; private set; }
        
        private void Connect()
        {
            var versionLabel = _rootVisualElement.Q<Label>("VersionLabel");
            versionLabel.text = _settings.version;
            
            LogLabel = _rootVisualElement.Q<Label>("LogLabel");
            
            ApiKeyTextField = _rootVisualElement.Q<TextField>("ApiKeyTextField");
            ApiKeyTextField.value = _settings.apiKey;
            ApiKeyTextField.RegisterCallback<InputEvent>((evt) =>
            {
                _settings.apiKey = ApiKeyTextField.value;
            });

            ContentFolderTextField = _rootVisualElement.Q<TextField>("ContentFolderTextField");
            ContentFolderTextField.value = _settings.contentFolderPath;
            ContentFolderTextField.RegisterCallback<InputEvent>((evt) =>
            {
                _settings.contentFolderPath = ContentFolderTextField.value;
            });
            
            BucketNameTextField = _rootVisualElement.Q<TextField>("BucketNameTextField");
            BucketNameTextField.RegisterCallback<InputEvent>((evt) =>
            {
                _bucketData.bucketName = BucketNameTextField.value;
            });
            
            LastReleaseIdTextField = _rootVisualElement.Q<TextField>("LastReleaseIdTextField");
            if (_bucketData != null && string.IsNullOrEmpty(_bucketData.lastReleaseId))
            {
                LastReleaseIdTextField.value = _bucketData.lastReleaseId;
            }
            
            BucketIdTextField = _rootVisualElement.Q<TextField>("BucketIdTextField");
            if (_bucketData != null && !string.IsNullOrEmpty(_bucketData.bucketId)) BucketIdTextField.value = _bucketData.bucketId;
        
            SelectFolderButton = _rootVisualElement.Q<Button>("SelectContentFolderButton");
            SelectFolderButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                ContentFolderTextField.value = GetFolderPath();
                _settings.contentFolderPath = ContentFolderTextField.value;
            });
            
            SaveChangesButton = _rootVisualElement.Q<Button>("SaveChangesButton");
            ClearLogButton = _rootVisualElement.Q<Button>("ClearLogButton");
            ClearLogButton.RegisterCallback<MouseUpEvent>((evt) => ClearLog());
            CreateReleaseButton = _rootVisualElement.Q<Button>("CreateReleaseButton");
            LoadLastReleaseButton = _rootVisualElement.Q<Button>("LoadLastReleaseButton");
            CreateBucketButton = _rootVisualElement.Q<Button>("CreateBucketButton");
            CheckBucketButton = _rootVisualElement.Q<Button>("CheckBucketButton");
            ShowFilesInBucketButton = _rootVisualElement.Q<Button>("ShowFilesInBucketButton");
            SynsContentButton = _rootVisualElement.Q<Button>("SynsContentButton");
            DeleteContentButton = _rootVisualElement.Q<Button>("DeleteContentButton");
            DeleteBucketButton = _rootVisualElement.Q<Button>("DeleteBucketButton");
        
            BucketObjectField = _rootVisualElement.Q<ObjectField>("BucketObjectField");
            BucketObjectField.objectType = typeof(BucketData);
            BucketObjectField.value = _settings.lastBucketData;
            BucketObjectField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                _bucketData = BucketObjectField.value as BucketData;
                _settings.lastBucketData = _bucketData;
                CheckBucketData();
            });
            
            CheckBucketData();
        }

        private void CheckBucketData()
        {
            if (_bucketData != null) InitBucketData();
            else HideBucketFields();
        }
        
        private void HideBucketFields()
        {
            BucketNameTextField.visible = false;
            BucketIdTextField.visible = false;
            CheckBucketButton.visible = false;
            ShowFilesInBucketButton.visible = false;
            LastReleaseIdTextField.visible = false;
            CreateBucketButton.visible = false;
            DeleteBucketButton.visible = false;
            SynsContentButton.visible = false;
            DeleteContentButton.visible = false;
            LoadLastReleaseButton.visible = false;
            CreateReleaseButton.visible = false;
        }

        public void InitBucketData()
        {
            BucketNameTextField.visible = true;
            BucketIdTextField.visible = true;
            CheckBucketButton.visible = true;
            ShowFilesInBucketButton.visible = true;
            LastReleaseIdTextField.visible = true;
            CreateBucketButton.visible = true;
            DeleteBucketButton.visible = true;
            SynsContentButton.visible = true;
            DeleteContentButton.visible = true;
            LoadLastReleaseButton.visible = true;
            CreateReleaseButton.visible = true;
        
            BucketNameTextField.value = _bucketData.bucketName;
            
            if (!string.IsNullOrEmpty(_bucketData.bucketId))
            {
                BucketIdTextField.value = _bucketData.bucketId;
            }
        }
        
        public void SaveAllChanges()
        {
            EditorUtility.SetDirty(_bucketData);
            EditorUtility.SetDirty(_settings);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    
        public void SaveChanges(Object obj)
        {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public void WriteLog(string s)
        {
            LogLabel.text = $"----------- \n" + LogLabel.text;
            LogLabel.text = $"{s} \n" + LogLabel.text;
        }
        
        public void ClearLog() => LogLabel.text = "";
        private string GetFolderPath() => EditorUtility.OpenFolderPanel("", "", "");
    }
}