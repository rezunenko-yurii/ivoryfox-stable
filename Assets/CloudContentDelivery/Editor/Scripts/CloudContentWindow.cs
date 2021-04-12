using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudContentDelivery.Runtime.Scripts;
using CloudContentDeliveryManagment.Api;
using CloudContentDeliveryManagment.Model;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CloudContentDelivery.Editor.Scripts
{
    public class CloudContentWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Cloud Content Delivery")]
        public static void ShowWindow() => GetWindow<CloudContentWindow>("Cloud Content");
    
        private CloudContentSettings _settings;
        private string _decodedApiKey;
        private BucketData _bucketData;
        private BucketsApi _bucketsApi;
        private EntriesApi _entriesApi;
        private ContentApi _contentApi;
        private ReleasesApi _releasesApi;
        private CcdWindowElements _windowElements;
    
        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("ContentDeliveryUI");
            visualTree.CloneTree(rootVisualElement);
            Connect();
            OnSelectionChange();
        }
        private void Connect()
        {
            _settings = Resources.Load<CloudContentSettings>("CloudContentSettings");
            _decodedApiKey = CcdHelper.Base64Encode($":{_settings.apiKey}");
        
            _bucketsApi = new BucketsApi();
            _entriesApi = new EntriesApi();
            _contentApi = new ContentApi();
            _releasesApi = new ReleasesApi();
            
            if (_settings.lastBucketData != null) _bucketData = _settings.lastBucketData;
            if (!_bucketsApi.Configuration.DefaultHeader.ContainsKey("Authorization")) _bucketsApi.Configuration.DefaultHeader.Add("Authorization", $"Basic {_decodedApiKey}");
            
            _windowElements = new CcdWindowElements(rootVisualElement, _settings, _bucketData);
            RegisterCallbacks();
        }
        private void RegisterCallbacks()
        {
            _windowElements.SaveChangesButton.RegisterCallback<MouseUpEvent>((evt) => _windowElements.SaveAllChanges());
            _windowElements.CreateReleaseButton.RegisterCallback<MouseUpEvent>((evt) => CreateRelease());
            _windowElements.LoadLastReleaseButton.RegisterCallback<MouseUpEvent>((evt) => LoadLastRelease());
            _windowElements.CreateBucketButton.RegisterCallback<MouseUpEvent>((evt) => CreateBucket());
            _windowElements.CheckBucketButton.RegisterCallback<MouseUpEvent>((evt) => CheckBucket());
            _windowElements.ShowFilesInBucketButton.RegisterCallback<MouseUpEvent>((evt) => ShowFilesBucket());
            _windowElements.SynsContentButton.RegisterCallback<MouseUpEvent>((evt) => SynchronizeContent());
            _windowElements.DeleteContentButton.RegisterCallback<MouseUpEvent>((evt) => DeleteCloudContent());
            _windowElements.DeleteBucketButton.RegisterCallback<MouseUpEvent>((evt) => DeleteBucket());
        }

        private void LoadLastRelease()
        {
            if (!string.IsNullOrEmpty(_bucketData.bucketId))
            {
                var releases = _releasesApi.GetReleases(_bucketData.bucketId);

                if (releases.Count <= 0) return;
                foreach (var release in releases)
                {
                    foreach (var badge in release.Badges)
                    {
                        if (!badge.Name.Equals("latest")) continue;
                        _bucketData.lastReleaseId = release.Releaseid.ToString();
                        _windowElements.LastReleaseIdTextField.value = _bucketData.lastReleaseId;
                        break;
                    }
                }
            }
            else _windowElements.WriteLog("bucket is null");
        }

        private void CreateRelease()
        {
            _windowElements.WriteLog("Start of Create Release");

            var rc = new ReleaseCreate();
            var releaseEntries = new List<ReleaseentryCreate>();
            
            var entries = _entriesApi.GetEntries(_bucketData.bucketId);
            foreach (var entry in entries)
            {
                var rec = new ReleaseentryCreate(entry.Entryid,entry.CurrentVersionid);
                releaseEntries.Add(rec);
            }

            rc.Entries = releaseEntries;
            var lastRelease = _releasesApi.CreateRelease(_bucketData.bucketId, rc);
            _bucketData.lastReleaseId = lastRelease.Releaseid.ToString();
            
            _windowElements.LastReleaseIdTextField.value = _bucketData.lastReleaseId;
            _windowElements.WriteLog($"Done | new release id {_bucketData.lastReleaseId}");
        }
        
        private void DeleteBucket()
        {
            _bucketsApi.DeleteBucket(_bucketData.bucketId);
            _bucketData.lastReleaseId = string.Empty;

            _windowElements.SaveChanges(_bucketData);
        }

        private void SynchronizeContent()
        {
            _windowElements.WriteLog("Start of Synchronizing Content");
        
            if (string.IsNullOrEmpty(_bucketData.bucketId)) CheckBucket();

            List<LocalContent> localContent = GetLocalContent();
            var cloudContent = _entriesApi.GetEntries(_bucketData.bucketId);
            var localNotInCloud = new List<LocalContent>();
            var localInCloud = new List<LocalContent>();
            var onlyInCloud = new List<Entry>();

            foreach (var content in localContent)
            {
                var hasEntry = cloudContent.Any(cc => cc.Path.Equals(content.GetPathInCloud));
                if(hasEntry) localInCloud.Add(content);
                else localNotInCloud.Add(content);
            }

            foreach (var entry in cloudContent)
            {
                var hasEntry = localContent.Any(lc => lc.GetPathInCloud.Equals(entry.Path));
                if (!hasEntry) onlyInCloud.Add(entry);
            }

            EntryOperation(localNotInCloud, "create");
            EntryOperation(localInCloud, "update");
            RemoveCloudContent(onlyInCloud);
        }

        private void RemoveCloudContent(List<Entry> onlyInCloud)
        {
            _windowElements.WriteLog($"RemoveCloudContent | count {onlyInCloud.Count}");
        
            foreach (var entry in onlyInCloud)
            {
                _entriesApi.DeleteEntry(_bucketData.bucketId, entry.Entryid.ToString());
                _windowElements.WriteLog($"Not Found local entry {entry.Path} {entry.Entryid} | It will be removed from cloud ");
            }
        }

        private void EntryOperation(List<LocalContent> localContent, string type)
        {
            _windowElements.WriteLog($"EntryOperation -- {type} | count {localContent.Count}");
        
            foreach (var local in localContent)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var fileStream = new FileStream(local.GetFilePath, FileMode.Open, FileAccess.ReadWrite)) { fileStream.CopyTo(memoryStream); }
                    memoryStream.Position = 0;

                    var checkSum = CcdHelper.GetCheckSum(memoryStream);
                    var  mime = MimeTypeMap.GetMimeType(local.GetFilePath);
                    var size = (int) memoryStream.Length;
                    var labels = new List<string>() {"string"};

                    if (type == "create") CreateEntry(local, checkSum, mime,size, labels);
                    else if(type == "update") UpdateEntry(local, checkSum, mime,size, labels);
                    else
                    {
                        _windowElements.WriteLog($"Can`t handle this type = {type}");
                        return;
                    }
                }
            }
        }

        private void CreateEntry(LocalContent local, string checkSum, string mime, int size, List<string> labels)
        {
            var ec = new EntryCreate(checkSum, size,mime , labels, null, local.GetPathInCloud);
            var entryResponse = _entriesApi.CreateEntry(_bucketData.bucketId, ec);
        
            _windowElements.WriteLog($"Created entry | path {entryResponse.Path} " + $"| hash {entryResponse.ContentHash} " + $"| size {entryResponse.ContentSize} " + $"| type {entryResponse.ContentType} ");
            Upload(local);
        }
    
        private void UpdateEntry(LocalContent local, string checkSum, string mime, int size, List<string> labels)
        {
            var entryForUpdate = _entriesApi.GetEntryByPath(_bucketData.bucketId, local.GetPathInCloud);
                    
            if (!checkSum.Equals(entryForUpdate.ContentHash))
            {
                var entryUpdate = new EntryUpdate(checkSum, size, mime, labels);
                var entryResponse = _entriesApi.UpdateEntry(_bucketData.bucketId,entryForUpdate.Entryid.ToString(), entryUpdate);
            
                _windowElements.WriteLog($"Updated entry | path {entryResponse.Path} " + $"| hash {entryResponse.ContentHash} " + $"| size {entryResponse.ContentSize} " + $"| type {entryResponse.ContentType} ");
            
                Upload(local);
            }
            else
            {
                _windowElements.WriteLog($"Skip updating | file not changed {local.GetPathInCloud}");
            }
        }
        
        private void Upload(LocalContent local)
        {
            var entry = _entriesApi.GetEntryByPath(_bucketData.bucketId, local.GetPathInCloud);
            
            if (entry == null) return;
            using (var memoryStream = new MemoryStream())
            {
                using (FileStream fileStream = new FileStream(local.GetFilePath, FileMode.Open, FileAccess.ReadWrite)) { fileStream.CopyTo(memoryStream); }
                memoryStream.Position = 0;

                _contentApi.UploadContent(_bucketData.bucketId,entry.Entryid.ToString(), memoryStream);
                _windowElements.WriteLog($"Uploaded entry {entry.Path} {entry.Entryid} {entry.CurrentVersionid}"); 
            }
        }

        private void DeleteCloudContent()
        {
            var cloudContent = _entriesApi.GetEntries(_bucketData.bucketId);
            if (cloudContent.Count > 0)
            {
                foreach (Entry entry in cloudContent)
                {
                    _entriesApi.DeleteEntry(_bucketData.bucketId, entry.Entryid.ToString());
                    _windowElements.WriteLog($"Deleted entry {entry.Path} from cloud ");
                }
                
                _bucketData.lastReleaseId = string.Empty;

            }
            else _windowElements.WriteLog("You don`t have cloud content");
        }

        private List<LocalContent> GetLocalContent()
        {
            string path = Application.dataPath + "/CloudContentDelivery/Editor/Resources/Content";
            var files = CcdHelper.FindAll(path, new []{".meta"}).ToList();
            List<LocalContent> localContent = new List<LocalContent>();
        
            foreach (var file in files) localContent.Add(new LocalContent(file));
            return localContent;
        }
        
        private void ShowFilesBucket()
        {
            if (string.IsNullOrEmpty(_bucketData.bucketId)) CheckBucket();

            var entriesList = _entriesApi.GetEntries(_bucketData.bucketId);
            if (entriesList.Count > 0)
            {
                foreach (Entry entry in entriesList)
                {
                    _windowElements.WriteLog($"Entry {entry.Path} " + $"| modified = {entry.LastModified}" + $"| id = {entry.Entryid}" + $"| size = {entry.ContentSize}");
                }
            }
            else _windowElements.WriteLog("No entries in bucket");
        }

        private bool CheckBucket()
        {
            bool found = false;
            
            var listBuckets = _bucketsApi.ListBucketsByProject(Application.cloudProjectId);
            foreach (var b in listBuckets)
            {
                if (_bucketData.bucketName.Equals(b.Name) || _bucketData.bucketId.Equals(b.Id.ToString()))
                {
                    _bucketData.bucketName = b.Name;
                    _bucketData.bucketId = b.Id.ToString();
                    _windowElements.InitBucketData();
                    LoadLastRelease();
                    
                    _windowElements.SaveChanges(_bucketData);
                    _windowElements.WriteLog($"Bucket {_bucketData.bucketName} exist");
                    found = true;
                    
                    break;
                }
            }

            if (!found)
            {
                _windowElements.WriteLog("Bucket is not found");
                return false;
            }

            return true;
        }
        private void CreateBucket()
        {
            _windowElements.WriteLog("Start Of Create Bucket");

            if (!CheckBucket())
            {
                var bucket = _bucketsApi.CreateBucketByProject(Application.cloudProjectId, new BucketCreate("", "IvoryFox Bucket", Guid.Parse(Application.cloudProjectId)));
                _windowElements.WriteLog($"Created Bucket {bucket.Name} \n");
            }
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
        
        private void OnDestroy()
        {
            _bucketData = null;
            _bucketsApi = null;
            _entriesApi = null;
            _contentApi = null;
            _releasesApi = null;
            
            Debug.Log("Destroy Cloud Content Window");
        }
    }
}
