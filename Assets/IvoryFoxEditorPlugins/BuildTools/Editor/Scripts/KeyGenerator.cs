using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Android;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BuildTools.Editor.Scripts
{
    public static class KeyGenerator
    {
        private static readonly string KeystorePath = $"{Application.dataPath}".Replace("/", "\\");
        private static readonly string KeytoolPath = $"{AndroidExternalToolsSettings.jdkRootPath.Replace("\\", "/")}/bin";

        public static void Create(string companyName, string keyName, string keyPass)
        {
            string pathToProcess =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
            
            string arguments = $"/c keytool -genkey -dname \"ou={companyName}\" " +
                               $"-alias {keyName.ToLowerInvariant()} -keypass {keyPass} " +
                               $"-keystore \"{KeystorePath}\\{keyName}.keystore\" " +
                               $"-storepass {keyPass} -keyalg RSA -keysize 2048 -validity 18250";
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = pathToProcess,
                        WorkingDirectory = KeytoolPath,
                        WindowStyle = ProcessWindowStyle.Normal,

                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        
                        Arguments = arguments
                    }
                };
                
                process.Start();
                process.WaitForExit(10000);
            }
            catch (Exception e)
            {
                Debug.LogError($"KeyGenerator error {e}");
                throw;
            }
        }
        
        public static bool DetectKey(string keyName)
        {
            string ext = $"{keyName}.keystore";
            var key = Directory.EnumerateFiles(KeystorePath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.Contains(ext));
            return key.ToList().Count > 0;
        }
    }
}