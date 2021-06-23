using System;
using System.IO;

namespace ReportTools.Editor.Scripts.ApkParser
{
    public static class ApkManifestReader
    {
        public static ApkInfo ReadApkFromPath(string apkPath, string apkSignerPath)
        {
            byte[] manifestData = null;
            byte[] resourcesData = null;
            
            using (var zip = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(apkPath)))
            {
                using (var fileStream = new FileStream(apkPath, FileMode.Open, FileAccess.Read))
                {
                    var zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(fileStream);
                    ICSharpCode.SharpZipLib.Zip.ZipEntry item;
                    
                    while ((item = zip.GetNextEntry()) != null)
                    {
                        if (item.Name.ToLower() == "androidmanifest.xml")
                        {
                            manifestData = new byte[50 * 1024];
                            using (var inputStream = zipFile.GetInputStream(item))
                            {
                                inputStream.Read(manifestData, 0, manifestData.Length);
                            }
                        }

                        if (item.Name.ToLower() == "resources.arsc")
                        {
                            using (var inputStream = zipFile.GetInputStream(item))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    inputStream.CopyTo(ms);
                                    resourcesData = ms.ToArray();
                                }
                            }
                        }

                        if (resourcesData != null && manifestData != null)
                            break;
                    }
                }
            }

            var info = new ApkReader(manifestData, resourcesData, apkPath, apkSignerPath).Info;
            
            return info;
        }
        
        public static byte[] ToArray(this Stream s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.CanRead)
                throw new ArgumentException("Stream cannot be read");

            MemoryStream ms = s as MemoryStream;
            if (ms != null)
                return ms.ToArray();

            long pos = s.CanSeek ? s.Position : 0L;
            if (pos != 0L)
                s.Seek(0, SeekOrigin.Begin);

            byte[] result = new byte[s.Length];
            s.Read(result, 0, result.Length);
            if (s.CanSeek)
                s.Seek(pos, SeekOrigin.Begin);
            return result;
        }
    }
}