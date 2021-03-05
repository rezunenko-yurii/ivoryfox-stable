using System;
using UnityEngine;

namespace GitHelper.Editor.Scripts
{
    [Serializable, CreateAssetMenu(fileName = "GitHelperData", menuName = "IvoryFox/Create GitHelperData")]
    public class GitHelperData : ScriptableObject
    {
        public string version;
        public string token;
        public string projectKey;
        public string repositoryName;
        public string repositorySlugName;
        public string repositoryUrl;
    }
}