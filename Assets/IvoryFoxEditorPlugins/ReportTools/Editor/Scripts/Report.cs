using System;
using System.IO;
using System.Linq;
using System.Text;
using ReportTools.Editor.Scripts.ApkParser;
using UnityEditor;
using UnityEngine;

namespace ReportTools.Editor.Scripts
{
    public static class Report
    {
        public static string ApkSignerPath
        {
            get
            {
                var directories = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Android\Sdk\build-tools"));
                return directories[directories.Length - 1];
            }
        }
        
        public static void CreateReport(string apkLocation)
        {
            var info = ApkManifestReader.ReadApkFromPath(apkLocation, ApkSignerPath);
            AnalyzeReport(info, apkLocation);
        }

        private static void AnalyzeReport(ApkInfo info, string apkLocation)
        {

            string apkName = apkLocation.Split(new[] {"/"}, StringSplitOptions.None).Last();
            
            var builder = new StringBuilder();
            builder.AppendLine("{color:#fc035e}*" + $"Release V{PlayerSettings.bundleVersion}" + "*{color} " + $"[^{apkName}]");
            //builder.AppendLine("{color:#fc035e}*" + $"Release V{PlayerSettings.bundleVersion}" + "*{color} " + $"[^{buildData.GetApkName}]");
            builder.AppendLine($"");
            
            builder.AppendLine("{panel:title=Manifest Info|borderStyle=dashed|borderColor=#ccc|titleBGColor=#F7D6C1|bgColor=#FFFFCE}");
            builder.AppendLine($"* Name = *{info.label}*");
            builder.AppendLine($"* packageID = {SetColorForBool(info.packageName,info.packageName.Equals("app.lustigovich.testadjusti"), false)}");
            builder.AppendLine($"* appVersion = *{info.versionCode}*");
            builder.AppendLine($"* minSdkVersion = {info.minSdkVersion}");
            builder.AppendLine($"* targetSdkVersion = {SetColorForBool(info.targetSdkVersion,Int32.Parse(info.targetSdkVersion) >= 30)}");
            builder.AppendLine($"* debuggable = {SetColorForBool(info.IsDebuggable, false)}");
            builder.AppendLine($"* AdjustAvailable = {SetColorForBool(info.adjustAvailable)}");
            builder.AppendLine($"* hasIcon = {SetColorForBool(info.hasIcon)}");
            builder.AppendLine($"* screenOrientation = {Enum.Parse(typeof(AndroidScreenOrientation),info.screenOrientation)}");
            builder.AppendLine($"* Permissions");
            foreach (string permission in info.permissions) builder.AppendLine($"** {permission}");
            builder.AppendLine($"* keysHash");
            builder.AppendLine($"** {info.keysHash[0]}");
            builder.Append($"** {info.keysHash[1]}");
            builder.AppendLine("{panel}");
            
            builder.AppendLine("{panel:title=Unity Settings|borderStyle=dashed|borderColor=#ccc|titleBGColor=#ACAEEB|bgColor=#D9E0FD}");
            builder.AppendLine($"* productName = *{PlayerSettings.productName}*");
            builder.AppendLine($"* companyName = {PlayerSettings.companyName}");
            builder.AppendLine($"* appVersion = *{PlayerSettings.bundleVersion}*");
            builder.AppendLine($"* packageID=*{SetColorForString(PlayerSettings.applicationIdentifier, "app.lustigovich.testadjusti", false)}*");
            builder.AppendLine($"* minSdkVersion = {PlayerSettings.Android.minSdkVersion}");
            builder.AppendLine($"* targetSdkVersion = {PlayerSettings.Android.targetSdkVersion}");
            builder.AppendLine($"* scriptingBackend = {PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android)}");
            builder.AppendLine($"* targetArchitectures = {SetColorForEnum(PlayerSettings.Android.targetArchitectures, AndroidArchitecture.ARM64)}");
            builder.AppendLine($"* debug = {SetColorForBool(EditorUserBuildSettings.development, false)}");
            builder.AppendLine($"* useCustomKeystore = {SetColorForBool(PlayerSettings.Android.useCustomKeystore)}");
            builder.AppendLine($"* keystoreName = {PlayerSettings.Android.keystoreName}");
            builder.AppendLine($"* keystorePass = {PlayerSettings.Android.keystorePass}");
            builder.AppendLine($"* keyaliasName = {PlayerSettings.Android.keyaliasName}");
            builder.AppendLine($"* keyaliasPass = {PlayerSettings.Android.keyaliasPass}");
            builder.Append($"* screenOrientation = {PlayerSettings.defaultInterfaceOrientation}");
            builder.AppendLine("{panel}");
            
            //string fileName = $"{GetFullApkPath(buildData, saveFolder)}.txt";
            string fileName = $"{apkLocation}.txt";
            
            WriteReport(fileName, builder);
            EditorUtility.RevealInFinder(fileName);
        }
        
        private static void WriteReport(string fileName, StringBuilder builder)
        {
            try    
            {
                if (File.Exists(fileName)) File.Delete(fileName);
                
                using (FileStream fs = File.Create(fileName))     
                {
                    byte[] title = new UTF8Encoding(true).GetBytes(builder.ToString());    
                    fs.Write(title, 0, title.Length);
                }
            }    
            catch (Exception ex)    
            {    
                Console.WriteLine(ex.ToString());    
            }
        }

        #region SetColor
        
        private static string SetColor(string value, Color color)
        {
            string template = "{{color:#{0}}}*{1}*{{color}}";
            return string.Format(template, ColorUtility.ToHtmlStringRGB(color), value);
        }
        private static string SetColorForBool(bool valueForCompare, bool checkValue = true)
        {
            Color color = valueForCompare == checkValue ? Color.blue : Color.red;
            return SetColor(valueForCompare.ToString(), color);
        }
        private static string SetColorForBool(string valueForPrint,bool valueForCompare, bool checkValue = true)
        {
            Color color = valueForCompare == checkValue ? Color.blue : Color.red;
            return SetColor(valueForPrint, color);
        }
        
        private static string SetColorForString(string value, string secondValue, bool checkValue = true)
        {
            Color color = string.Equals(value, secondValue) == checkValue ? Color.blue : Color.red;
            return SetColor(value, color);
        }
        private static string SetColorForEnum(Enum value, Enum secondValue)
        {
            Color color = value.HasFlag(secondValue) ? Color.blue : Color.red;
            return SetColor(value.ToString(), color);
        }
        
        #endregion
    }
}