using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bitbucket.Net;
using Bitbucket.Net.Models.Core.Projects;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GitHelper.Editor.Scripts
{
    public class GitHelperWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Git Helper")]
        public static void ShowWindow() => GetWindow<GitHelperWindow>("Git Helper");

        private GitHelperData data;
        private TextField _messageField;
        private TextField _slugNameField;
        private TextField _repoUrlField;
        private BitbucketClient _client;

        private const string GitHelperFirstInit = "GitHelperFirstInit";
        [InitializeOnLoadMethod]
        [MenuItem("IvoryFox/Git Helper/First Init")]
        private static void FirstInit()
        {
            /*int d = PlayerPrefs.GetInt(GitHelperFirstInit, 0);
            bool isInited = Convert.ToBoolean(d);*/

            string gitFolderPath = Application.dataPath.Replace("/Assets", "");
            gitFolderPath = $"{gitFolderPath}/.git";
            
            if (!Directory.Exists(gitFolderPath))
            {
                GitCommands.Instance().SetGitIgnore();
                GitCommands.Instance().InitGit();
                
                PlayerSettings.assemblyVersionValidation = false;
            }
            
            /*if (!isInited)
            {
                GitCommands.Instance().SetGitIgnore();
                
                string gitFolderPath = Application.dataPath.Replace("/Assets", "");
                gitFolderPath = $"{gitFolderPath}/.git";
                if (Directory.Exists(gitFolderPath))
                {
                    GitCommands.Instance().InitGit();
                }
                
                PlayerSettings.assemblyVersionValidation = false;
                PlayerPrefs.SetInt(GitHelperFirstInit, 1);
            }*/
        }

        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("GitHelperUI");
            visualTree.CloneTree(rootVisualElement);
            
            Connect();
            //OnSelectionChange();
        }
        
        private void Connect()
        {
            data = Resources.Load<GitHelperData>("GitHelperData");

            if (data is null)
            {
                Debug.LogError("Cannot find GitHelperData.asset // Create it in PackageManagerAssets/Resources/GitHelperData.asset");
                return;
            }
            
            _messageField = rootVisualElement.Q<TextField>("MessageField");
          
            var version = rootVisualElement.Q<Label>("VersionLabel");
            version.text = data.version;

            var gitProfileObjectField = rootVisualElement.Q<ObjectField>("GitProfileObjectField");
            gitProfileObjectField.objectType = typeof(GitProfile);
            gitProfileObjectField.value = data.lastGitProfile;
            gitProfileObjectField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            gitProfileObjectField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                data.lastGitProfile = (GitProfile) evt.newValue;
            });
            
            if(data.lastGitProfile is null) return;

            LoadRepositoryData();
            
            var tokenField = rootVisualElement.Q<TextField>("TokenField");
            tokenField.value = data.lastGitProfile.token;
            tokenField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            tokenField.RegisterCallback<InputEvent>((evt) => data.lastGitProfile.token = evt.newData);
            
            var repoNameField = rootVisualElement.Q<TextField>("NameField");
            repoNameField.value = data.lastRepositoryData.repositoryName;
            repoNameField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            repoNameField.RegisterCallback<InputEvent>((evt) => data.lastRepositoryData.repositoryName = evt.newData);
            
            _slugNameField = rootVisualElement.Q<TextField>("SlugNameField");
            _slugNameField.value = data.lastRepositoryData.repositorySlugName;
            _slugNameField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            _slugNameField.RegisterCallback<InputEvent>((evt) => data.lastRepositoryData.repositorySlugName = evt.newData);
            
            _repoUrlField = rootVisualElement.Q<TextField>("RepoUrlField");
            _repoUrlField.value = data.lastRepositoryData.repositoryUrl;
            _repoUrlField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            _repoUrlField.RegisterCallback<InputEvent>((evt) => data.lastRepositoryData.repositoryUrl = evt.newData);
            
            var repoUrlButton = rootVisualElement.Q<Button>("SetUrlButton");
            repoUrlButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().SetRemoteBranch(data.lastRepositoryData.repositoryUrl));
            
            var projectKeyField = rootVisualElement.Q<TextField>("ProjectKeyField");
            projectKeyField.value = data.lastGitProfile.projectKey;
            projectKeyField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            projectKeyField.RegisterCallback<InputEvent>((evt) => data.lastGitProfile.projectKey = evt.newData);
            
            var autoSetTaskNumberToggle = rootVisualElement.Q<Button>("GetTitleButton");
            autoSetTaskNumberToggle.RegisterCallback<MouseUpEvent>((evt) =>
            {
                repoNameField.value = SetRepoNameFromTitle();
                _slugNameField.value = repoNameField.value.Replace(" ", "-");

                data.lastRepositoryData.repositoryName = repoNameField.value;
                data.lastRepositoryData.repositorySlugName = _slugNameField.value;
                
                SaveData();
            });

            var createButton = rootVisualElement.Q<Button>("CreateButton");
            createButton.RegisterCallback<MouseUpEvent>((evt) => CreateProjectRepository());

            var pushButton = rootVisualElement.Q<Button>("PushButton");
            pushButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().Push());
            
            var commitButton = rootVisualElement.Q<Button>("CommitButton");
            commitButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().CommitChanges(_messageField.value));
        }

        private void LoadRepositoryData()
        {
            if(data.lastRepositoryData != null) return;
            
            RepositoryData repositoryData = Resources.Load<RepositoryData>("CurrentRepositoryData");

            if (repositoryData is null)
            {
                string packageManagerFolder = Application.dataPath + "/PackageManagerAssets/Resources";
                string repositoryDataPath = "Assets/PackageManagerAssets/Resources/CurrentRepositoryData.asset";

                if (!Directory.Exists(packageManagerFolder)) Directory.CreateDirectory(packageManagerFolder);
                
                repositoryData = CreateInstance<RepositoryData>();
                AssetDatabase.CreateAsset(repositoryData, repositoryDataPath);
                AssetDatabase.SaveAssets();
            }

            data.lastRepositoryData = repositoryData;
        }

        private void SaveData()
        {
            EditorUtility.SetDirty(data);
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void Auth()
        {
            if (_client is null)
            {
                if (!string.IsNullOrEmpty(data.lastGitProfile.token))
                {
                    _client = new BitbucketClient("https://git.syneforge.com/", ()=> data.lastGitProfile.token);
                    WriteLog(_client != null ? "Successfully authorized" : "Authorization failed");
                }
                else WriteLog("Token is empty");
            }
            else WriteLog("Already authorized");
        }
        
        private string SetRepoNameFromTitle()
        {
            string[] s = Application.dataPath.Split('/');
            string projectName = s[s.Length - 2];
            return projectName;
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

        private async void CreateProjectRepository()
        {
            Auth();
            
            var exists = await IsRemoteRepoExists();
            if (!exists)
            {
                var newRepository = await CreateRepo();
                if (newRepository != null)
                {
                    WriteLog("New Repo Successfully created");
                    _slugNameField.value = newRepository.Slug;
                    data.lastRepositoryData.repositorySlugName = newRepository.Slug;
                    SaveData();
                
                    string link = newRepository.Links.Clone.FirstOrDefault(cloneLink => cloneLink.Href.Contains("https"))?.Href;
                    data.lastRepositoryData.repositoryUrl = link;
                    _repoUrlField.value = link;
                    
                    GitCommands.Instance().AddRemoteBranch(link); 
                }
                else WriteLog("Something went wrong. New repo not created");
            }
            else WriteLog("Repo already exists");
        }

        private async Task<bool> IsRemoteRepoExists()
        {
            if (!string.IsNullOrEmpty(data.lastGitProfile.projectKey) && !string.IsNullOrEmpty(data.lastRepositoryData.repositorySlugName))
            {
                WriteLog("Trying to get repository...");
                Task<Repository> remoteRepo;
                try
                {
                    remoteRepo = _client.GetProjectRepositoryAsync(data.lastGitProfile.projectKey, data.lastRepositoryData.repositorySlugName);
                    await remoteRepo;
                }
                catch (Exception e)
                {
                    WriteLog("Can`t check repository...");
                    WriteLog(e.Message);
                    return false;
                }
                
                while (!remoteRepo.IsCompleted)
                {
                    await Task.Yield();
                }

                return !(remoteRepo.Result is null);
            }
            else
            {
                WriteLog("Can`t check remote repository. Repository name or slug name is empty");
                return false;
            }
        }

        private async Task<Repository> CreateRepo()
        {
            if (!string.IsNullOrEmpty(data.lastGitProfile.projectKey) && !string.IsNullOrEmpty(data.lastRepositoryData.repositoryName))
            {
                WriteLog("Creating of new remote repository...");
                Task<Repository> newRepo;
                try
                {
                    newRepo = _client.CreateProjectRepositoryAsync(data.lastGitProfile.projectKey, data.lastRepositoryData.repositoryName);
                    await newRepo;
                }
                catch (Exception e)
                {
                    WriteLog(e.Message);
                    return null;
                }
                
                while (!newRepo.IsCompleted)
                {
                    await Task.Yield();
                }

                return newRepo.Result;
            }
            else
            {
                WriteLog("Can`t create remote repository. Repository name or project key is empty");
                return null;
            }
        }
        private void WriteLog(string s)
        {
            Debug.Log(s);
            Debug.Log("-----------");
        }
    }
}
