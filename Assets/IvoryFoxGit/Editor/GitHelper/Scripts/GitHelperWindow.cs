using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bitbucket.Net;
using Bitbucket.Net.Models.Core.Projects;
using IvoryFoxGit.Editor.GitCore.Scripts;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace IvoryFoxGit.Editor.GitHelper.Scripts
{
    public class GitHelperWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Git Helper Window")]
        public static void ShowWindow() => GetWindow<GitHelperWindow>("Git Helper");

        private GitHelperData _gitHelperData;
        private TextField _messageField;
        private TextField _repoNameField;
        private TextField _slugNameField;
        private TextField _repoUrlField;
        private BitbucketClient _bitbucketClient;
        
        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("GitHelperUI");
            visualTree.CloneTree(rootVisualElement);
            
            Connect();
        }
        
        private void Connect()
        {
            _gitHelperData = Resources.Load<GitHelperData>("GitHelperData");

            if (_gitHelperData is null)
            {
                Debug.LogError("Cannot find GitHelperData.asset // Create it in PackageManagerAssets/Resources/GitHelperData.asset");
                return;
            }
            
            _messageField = rootVisualElement.Q<TextField>("MessageField");
          
            var version = rootVisualElement.Q<Label>("VersionLabel");
            version.text = _gitHelperData.version;

            var gitProfileObjectField = rootVisualElement.Q<ObjectField>("GitProfileObjectField");
            gitProfileObjectField.objectType = typeof(GitProfile);
            gitProfileObjectField.value = _gitHelperData.lastGitProfile;
            gitProfileObjectField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            gitProfileObjectField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                _gitHelperData.lastGitProfile = (GitProfile) evt.newValue;
            });
            
            if(_gitHelperData.lastGitProfile is null) return;

            LoadRepositoryData();
            
            var tokenField = rootVisualElement.Q<TextField>("TokenField");
            tokenField.value = _gitHelperData.lastGitProfile.token;
            tokenField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            tokenField.RegisterCallback<InputEvent>((evt) => _gitHelperData.lastGitProfile.token = evt.newData);
            
            _repoNameField = rootVisualElement.Q<TextField>("NameField");
            _repoNameField.value = _gitHelperData.lastRepositoryData.repositoryName;
            _repoNameField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            _repoNameField.RegisterCallback<InputEvent>((evt) =>
            {
                _gitHelperData.lastRepositoryData.repositoryName = evt.newData;
                ConvertToSlugButton();
            });
            
            _slugNameField = rootVisualElement.Q<TextField>("SlugNameField");
            _slugNameField.value = _gitHelperData.lastRepositoryData.repositorySlugName;
            _slugNameField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            _slugNameField.RegisterCallback<InputEvent>((evt) => _gitHelperData.lastRepositoryData.repositorySlugName = evt.newData);
            
            _repoUrlField = rootVisualElement.Q<TextField>("RepoUrlField");
            _repoUrlField.value = _gitHelperData.lastRepositoryData.repositoryUrl;
            _repoUrlField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            _repoUrlField.RegisterCallback<InputEvent>((evt) => _gitHelperData.lastRepositoryData.repositoryUrl = evt.newData);
            
            var convertToSlugButton = rootVisualElement.Q<Button>("ConvertToSlugButton");
            convertToSlugButton.RegisterCallback<MouseUpEvent>((evt) =>
            {
                ConvertToSlugButton();
            });
            
            var repoUrlButton = rootVisualElement.Q<Button>("SetUrlButton");
            repoUrlButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().SetRemoteBranch(_gitHelperData.lastRepositoryData.repositoryUrl));
            
            var projectKeyField = rootVisualElement.Q<TextField>("ProjectKeyField");
            projectKeyField.value = _gitHelperData.lastGitProfile.projectKey;
            projectKeyField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            projectKeyField.RegisterCallback<InputEvent>((evt) => _gitHelperData.lastGitProfile.projectKey = evt.newData);
            
            var autoSetTaskNumberToggle = rootVisualElement.Q<Button>("GetTitleButton");
            autoSetTaskNumberToggle.RegisterCallback<MouseUpEvent>((evt) =>
            {
                _repoNameField.value = SetRepoNameFromTitle();
                _gitHelperData.lastRepositoryData.repositoryName = _repoNameField.value;
                
                ConvertToSlugButton();
                SaveData();
            });

            var createButton = rootVisualElement.Q<Button>("CreateButton");
            createButton.RegisterCallback<MouseUpEvent>((evt) => CreateProjectRepository());

            var pushButton = rootVisualElement.Q<Button>("PushButton");
            pushButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().Push());
            
            var commitButton = rootVisualElement.Q<Button>("CommitButton");
            commitButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().CommitChanges(_messageField.value));
        }

        private void ConvertToSlugButton()
        {
            _slugNameField.value = _repoNameField.value.Replace(" ", "-");
            _gitHelperData.lastRepositoryData.repositorySlugName = _slugNameField.value;
        }

        private void LoadRepositoryData()
        {
            if(_gitHelperData.lastRepositoryData != null) return;
            
            var repositoryData = Resources.Load<RepositoryData>("CurrentRepositoryData");

            if (repositoryData is null)
            {
                string packageManagerFolder = Application.dataPath + "/PackageManagerAssets/Resources";
                string repositoryDataPath = "Assets/PackageManagerAssets/Resources/CurrentRepositoryData.asset";

                if (!Directory.Exists(packageManagerFolder)) Directory.CreateDirectory(packageManagerFolder);
                
                repositoryData = CreateInstance<RepositoryData>();
                AssetDatabase.CreateAsset(repositoryData, repositoryDataPath);
                AssetDatabase.SaveAssets();
            }

            _gitHelperData.lastRepositoryData = repositoryData;
        }

        private void SaveData()
        {
            EditorUtility.SetDirty(_gitHelperData);
            EditorUtility.SetDirty(_gitHelperData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void Auth()
        {
            if (_bitbucketClient is null)
            {
                if (!string.IsNullOrEmpty(_gitHelperData.lastGitProfile.token))
                {
                    _bitbucketClient = new BitbucketClient("https://git.syneforge.com/", ()=> _gitHelperData.lastGitProfile.token);
                    WriteLog(_bitbucketClient != null ? "Successfully authorized" : "Authorization failed");
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
                    _gitHelperData.lastRepositoryData.repositorySlugName = newRepository.Slug;
                    SaveData();
                
                    string link = newRepository.Links.Clone.FirstOrDefault(cloneLink => cloneLink.Href.Contains("https"))?.Href;
                    _gitHelperData.lastRepositoryData.repositoryUrl = link;
                    _repoUrlField.value = link;
                    
                    GitCommands.Instance().AddRemoteBranch(link); 
                }
                else WriteLog("Something went wrong. New repo not created");
            }
            else WriteLog("Repo already exists");
        }

        private async Task<bool> IsRemoteRepoExists()
        {
            if (!string.IsNullOrEmpty(_gitHelperData.lastGitProfile.projectKey) && !string.IsNullOrEmpty(_gitHelperData.lastRepositoryData.repositorySlugName))
            {
                WriteLog("Trying to get repository...");
                //Task<Repository> remoteRepo;
                try
                {
                    /*remoteRepo = _bitbucketClient.GetProjectRepositoryAsync(_gitHelperData.lastGitProfile.projectKey, _gitHelperData.lastRepositoryData.repositorySlugName);
                    remoteRepo = new Task<Repository>(() =>
                    {
                        _bitbucketClient.GetProjectRepositoryAsync(_gitHelperData.lastGitProfile.projectKey,
                            _gitHelperData.lastRepositoryData.repositorySlugName);
                        
                        return null;
                    });
                    remoteRepo.Start();
                    await remoteRepo;*/
                    
                    var repo = await _bitbucketClient.GetProjectRepositoryAsync(_gitHelperData.lastGitProfile.projectKey,
                        _gitHelperData.lastRepositoryData.repositorySlugName);
                    return !(repo is null);
                }
                catch (Exception e)
                {
                    WriteLog("Can`t check repository...");
                    WriteLog(e.Message);
                    return false;
                }
                
                /*Stopwatch stopwatch = Stopwatch.StartNew();
                while (!remoteRepo.IsCompleted)
                {
                    if (stopwatch.ElapsedMilliseconds < 10000)
                    {
                        await Task.Yield();
                    }
                    else
                    {
                        stopwatch.Stop();
                        WriteLog("Can`t check repo / waiting time is out");
                        break;
                    }
                }

                return !(remoteRepo.Result is null);*/
            }
            else
            {
                WriteLog("Can`t check remote repository. Repository name or slug name is empty");
                return false;
            }
        }

        private async Task<Repository> CreateRepo()
        {
            if (!string.IsNullOrEmpty(_gitHelperData.lastGitProfile.projectKey) && !string.IsNullOrEmpty(_gitHelperData.lastRepositoryData.repositoryName))
            {
                WriteLog("Creating of new remote repository...");
      
                try
                {
                    Repository newRepo = await _bitbucketClient.CreateProjectRepositoryAsync(_gitHelperData.lastGitProfile.projectKey, _gitHelperData.lastRepositoryData.repositoryName);
                    return newRepo;
                    //await newRepo;
                }
                catch (Exception e)
                {
                    WriteLog(e.Message);
                    return null;
                }
                /*Stopwatch stopwatch = Stopwatch.StartNew();
                /*while (!newRepo.IsCompleted)
                {
                    if (stopwatch.ElapsedMilliseconds < 10000)
                    {
                        await Task.Yield();
                    }
                    else
                    {
                        stopwatch.Stop();
                        WriteLog("Can`t create repo / waiting time is out");
                        break;
                    }
                }#1#

                return newRepo.Result;*/
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
