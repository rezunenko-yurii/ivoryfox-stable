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

namespace GitHelper.Editor.Scripts
{
    public class GitHelperWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Git Helper")]
        public static void ShowWindow() => GetWindow<GitHelperWindow>("Git Helper");

        private GitHelperData data;
        private TextField messageField;
        private TextField slugNameField;
        private TextField repoUrlField;
        private BitbucketClient client;

        private const string GitHelperFirstInit = "GitHelperFirstInit";
        [InitializeOnLoadMethod]
        private static void FirstInit()
        {
            int d = PlayerPrefs.GetInt(GitHelperFirstInit, 0);
            bool isInited = Convert.ToBoolean(d);

            if (!isInited)
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
            }
        }

        public void OnEnable()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("GitHelperUI");
            visualTree.CloneTree(rootVisualElement);
            
            Connect();
            OnSelectionChange();
        }
        
        private void Connect()
        {
            data = Resources.Load<GitHelperData>("GitHelperData");
            messageField = rootVisualElement.Q<TextField>("MessageField");
          
            var version = rootVisualElement.Q<Label>("VersionLabel");
            version.text = data.version;
            
            var tokenField = rootVisualElement.Q<TextField>("TokenField");
            tokenField.value = data.token;
            tokenField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            tokenField.RegisterCallback<InputEvent>((evt) => data.token = evt.newData);
            
            var repoNameField = rootVisualElement.Q<TextField>("NameField");
            repoNameField.value = data.repositoryName;
            repoNameField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            repoNameField.RegisterCallback<InputEvent>((evt) => data.repositoryName = evt.newData);
            
            slugNameField = rootVisualElement.Q<TextField>("SlugNameField");
            slugNameField.value = data.repositorySlugName;
            slugNameField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            slugNameField.RegisterCallback<InputEvent>((evt) => data.repositorySlugName = evt.newData);
            
            repoUrlField = rootVisualElement.Q<TextField>("RepoUrlField");
            repoUrlField.value = data.repositoryUrl;
            repoUrlField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            repoUrlField.RegisterCallback<InputEvent>((evt) => data.repositoryUrl = evt.newData);
            
            var repoUrlButton = rootVisualElement.Q<Button>("SetUrlButton");
            repoUrlButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().SetRemoteBranch(data.repositoryUrl));
            
            var projectKeyField = rootVisualElement.Q<TextField>("ProjectKeyField");
            projectKeyField.value = data.projectKey;
            projectKeyField.RegisterCallback<FocusOutEvent>(evt => SaveData());
            projectKeyField.RegisterCallback<InputEvent>((evt) => data.projectKey = evt.newData);
            
            var autoSetTaskNumberToggle = rootVisualElement.Q<Button>("GetTitleButton");
            autoSetTaskNumberToggle.RegisterCallback<MouseUpEvent>((evt) => slugNameField.value = SetRepoNameFromTitle());

            var createButton = rootVisualElement.Q<Button>("CreateButton");
            createButton.RegisterCallback<MouseUpEvent>((evt) => CreateProjectRepository());

            var pushButton = rootVisualElement.Q<Button>("PushButton");
            pushButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().Push());
            
            var commitButton = rootVisualElement.Q<Button>("CommitButton");
            commitButton.RegisterCallback<MouseUpEvent>((evt) => GitCommands.Instance().CommitChanges(messageField.value));
        }

        private void SaveData()
        {
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void Auth()
        {
            if (client is null)
            {
                if (!string.IsNullOrEmpty(data.token))
                {
                    client = new BitbucketClient("https://git.syneforge.com/", ()=> data.token);
                    WriteLog(client != null ? "Successfully authorized" : "Authorization failed");
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
                    slugNameField.value = newRepository.Slug;
                    data.repositorySlugName = newRepository.Slug;
                    SaveData();
                
                    string link = newRepository.Links.Clone.FirstOrDefault(cloneLink => cloneLink.Href.Contains("https"))?.Href;
                    data.repositoryUrl = link;
                    repoUrlField.value = link;
                    
                    GitCommands.Instance().AddRemoteBranch(link); 
                }
                else WriteLog("Something went wrong. New repo not created");
            }
            else WriteLog("Repo already exists");
        }

        private async Task<bool> IsRemoteRepoExists()
        {
            if (!string.IsNullOrEmpty(data.projectKey) && !string.IsNullOrEmpty(data.repositorySlugName))
            {
                WriteLog("Trying to get repository...");
                Task<Repository> remoteRepo;
                try
                {
                    remoteRepo = client.GetProjectRepositoryAsync(data.projectKey, data.repositorySlugName);
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
            if (!string.IsNullOrEmpty(data.projectKey) && !string.IsNullOrEmpty(data.repositoryName))
            {
                WriteLog("Creating of new remote repository...");
                Task<Repository> newRepo;
                try
                {
                    newRepo = client.CreateProjectRepositoryAsync(data.projectKey, data.repositoryName);
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
