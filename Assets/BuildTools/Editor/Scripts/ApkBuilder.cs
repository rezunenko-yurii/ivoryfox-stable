using System;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    public static class ApkBuilder
    {
        public static void BuildApk(BuildData data, string buildsFolderPath)
        {
            ManifestsChecker.Inspect();
            PlayerHelper.PrintPlayerSettings();
            CheckIcons();
            if (!data.development && !KeyGenerator.DetectKey("STPN-" + data.taskNumber)) Debug.LogError("Нет ключа");

            string buildPath = $"{buildsFolderPath}\\{data.GetApkName}";
            Debug.Log($"<color=green>Полный путь apk {buildPath}</color>");
            
            var buildPlayerOptions = new BuildPlayerOptions
            {
                target = BuildTarget.Android,
                options = data.buildOptions,
                scenes = GetScenePaths(),
                locationPathName = buildPath
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            Debug.Log($"Build {(report.summary.result == BuildResult.Succeeded ? $"<color=green>{report.summary.result.ToString()}</color>" : $"<color=red>{report.summary.result.ToString()}</color>")}");
            
            if (report.summary.result != BuildResult.Succeeded) return;
            
            EditorUtility.RevealInFinder(buildPath);
        }

        private static void CheckIcons()
        {
            var platform = BuildTargetGroup.Android;
            var kind = AndroidPlatformIconKind.Legacy;

            var icons = PlayerSettings.GetPlatformIcons(platform, kind);

            if (icons.Length <= 0)
            {
                Debug.LogError("Не выбрана иконка приложения");
                throw new Exception();
            }
            
            Debug.Log($"<color=green>Иконки присувствуют</color>");
        }
        private static string[] GetScenePaths() 
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            
            if (scenes.Length <= 0)
            {
                Debug.LogError("Нет добавленых сцен");
                throw new Exception();
            }
            
            Debug.Log($"<color=green>Сцены</color>");
            for(int i = 0; i < scenes.Length; i++) 
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
                Debug.Log(scenes[i]);
            }
            return scenes;
        }
    }
}