using System.IO;
using IvoryFoxGit.Editor.GitCore.Scripts;
using UnityEditor;
using UnityEngine;

namespace IvoryFoxGit.Editor.GitInit.Scripts
{
    public class FirstInitHelper
    {
        [InitializeOnLoadMethod, MenuItem("IvoryFox/Git Helper/First Init")]
        private static void FirstInit()
        {
            PlayerSettings.assemblyVersionValidation = false;
            
            string gitFolderPath = Application.dataPath.Replace("/Assets", "");
            gitFolderPath = $"{gitFolderPath}/.git";
            
            if (!Directory.Exists(gitFolderPath))
            {
                GitCommands.Instance().SetGitIgnore();
                GitCommands.Instance().InitGit();
            }
        }
    }
}
