using System;
using System.Collections;
using System.Collections.Generic;
using ExtendedPackageManager.Editor.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PackageManagerWindow : EditorWindow
{
    [MenuItem("IvoryFox/Package Manager")]
    public static void ShowWindow() => GetWindow<PackageManagerWindow>("Package Manager");

    private Package[] all;
    private void Awake()
    {
        all = Resources.LoadAll<Package>("");
    }

    public void OnEnable()
    {
        //var visualTree = Resources.Load<VisualTreeAsset>("PackageManagerUI");
        //visualTree.CloneTree(rootVisualElement);
        //Connect();
        //OnSelectionChange();
    }

    private void Connect()
    {
        var buildToolsVersionLabel = rootVisualElement.Q<ScrollView>("ScrollContainer");
        var line = rootVisualElement.Q<VisualElement>("PackageContainer");
        var a = new VisualElement.UxmlFactory();
        //IUxmlAttributes f = new Uxml
        //a.Create()
            
            
    }

    private void OnGUI()
    {
        var manifest = ManifestHelper.GetInstance();
        
        EditorGUILayout.BeginScrollView(new Vector2(100,400));
        
        foreach (Package package in all)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(package.packageName);
            
            if (manifest.Contains(package.packageId))
            {
                GUI.backgroundColor = Color.yellow;
                if (GUILayout.Button("Update"))
                {
                    package.TryToUpdate();
                }
                
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove"))
                {
                    package.Remove();
                }
            }
            else
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Install"))
                {
                    package.Install();
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
        }
        
        EditorGUILayout.EndScrollView();
    }
}
