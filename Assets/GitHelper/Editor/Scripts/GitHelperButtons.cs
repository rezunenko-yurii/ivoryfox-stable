using System;
using ToolbarExtenderPlugin.Editor.Scripts;
using UnityEditor;
using UnityEngine;

namespace GitHelper.Editor.Scripts
{
    [InitializeOnLoad]
    public class GitHelperButtons
    {
        static GitHelperButtons()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            GUI.color= Color.yellow;
            if(GUILayout.Button(new GUIContent("Commit", "Start Scene 1")))
            {
                GitCommands.Instance().CommitChanges();
            }
            GUI.color= Color.green;
            if(GUILayout.Button(new GUIContent("Push", "Start Scene 2")))
            {
                GitCommands.Instance().Push();
            }
        }
    }
}