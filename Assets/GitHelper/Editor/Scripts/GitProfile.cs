using UnityEngine;

namespace GitHelper.Editor.Scripts
{
    [CreateAssetMenu(fileName = "GitProfile", menuName = "IvoryFox/GitHelper/Create Git Profile", order = 0)]
    public class GitProfile : ScriptableObject
    {
        public string userName;
        public string token;
        public string projectKey;
    }
}