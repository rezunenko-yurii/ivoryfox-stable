using UnityEngine;

namespace IvoryFoxGit.Editor.GitHelper.Scripts
{
    [CreateAssetMenu(fileName = "RepositoryData", menuName = "IvoryFox/GitHelper/Create RepositoryData", order = 0)]
    public class RepositoryData : ScriptableObject
    {
        public string repositoryName;
        public string repositorySlugName;
        public string repositoryUrl;
    }
}