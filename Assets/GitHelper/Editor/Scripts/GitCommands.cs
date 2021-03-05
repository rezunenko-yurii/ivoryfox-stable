using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GitHelper.Editor.Scripts
{
    public class GitCommands
    {
        private string rootDirectory;
        private string cmd;

        private static GitCommands instance;

        public static GitCommands Instance()
        {
            if (instance is null)
            {
                instance = new GitCommands();
            }

            return instance;
        }
    
        private GitCommands()
        {
            rootDirectory = Path.GetDirectoryName(Application.dataPath);
            cmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
        }
    
        private void StartCmd(string arguments)
        {
            Debug.Log($"New Commands: {arguments}");
            try
            {
                Process proc = new Process
                {
                    StartInfo =
                    {
                        WorkingDirectory = rootDirectory,
                        FileName = cmd,
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        Arguments = arguments
                    }
                };
                
                proc.Start();
                var output = proc.StandardOutput.ReadToEnd();
                var error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                WriteLog(error);
                WriteLog(output);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace);
                WriteLog(ex.Message);
            }
        }
    
        public void InitGit()
        {
            WriteLog("Git init starts...");
            
            string s = "/k git init" 
                       + " & pause";
            StartCmd(s);
        }
    
        public void SetRemoteBranch(string link)
        {
            WriteLog($"Set remote branch {link} starts...");
            
            string s = $"/k git remote set-url origin {link}" 
                       + " & pause";
            StartCmd(s);
        }
        
        public void AddRemoteBranch(string link)
        {
            WriteLog($"Set remote branch {link} starts...");
            
            string s = $"/k git remote add origin {link}" 
                       + " & pause";
            StartCmd(s);
        }
    
        public void SetGitIgnore()
        {
            WriteLog("Adding git ignore");
            
            var a = Resources.Load<TextAsset>("gitignore");
            var root = Path.GetDirectoryName(Application.dataPath);
            File.WriteAllText(root + "\\.gitignore", a.text);
        }
    
        public void Push()
        {
            WriteLog("Trying to push...");
            
            string s = "/k git push -u origin master" 
                       + " & pause";
            StartCmd(s);
            WriteLog("Changes is pushed");
        }
    
        public void CommitChanges()
        {
            CommitChanges(DateTime.Now.ToString());
        }
    
        public void CommitChanges(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                WriteLog("Trying to commit...");
                
                string s = "/k git add -A" 
                           + $" && git commit -m \"{message}\"" 
                           + " & pause";
                StartCmd(s); 
                WriteLog("Commit created");
            }
            else WriteLog("Message is empty");
        }

        private void WriteLog(string s)
        {
            Debug.Log(s);
            Debug.Log("-----------");
        }
    }
}
