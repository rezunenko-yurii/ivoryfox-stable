using System;
using UnityEngine;

namespace IvoryFoxGit.Editor.GitHelper.Scripts
{
    [Serializable, CreateAssetMenu(fileName = "GitHelperData", menuName = "IvoryFox/GitHelper/Create GitHelperData")]
    public class GitHelperData : ScriptableObject
    {
        public string version;
        public GitProfile lastGitProfile;
        public RepositoryData lastRepositoryData;
        
        /*public string token;
        public string projectKey;
        public string repositoryName;
        public string repositorySlugName;
        public string repositoryUrl;*/
    }
}