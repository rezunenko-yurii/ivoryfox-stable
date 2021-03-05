using System;
using System.IO;
using System.Linq;

namespace BuildTools.Editor.Scripts.ApkParser
{
    public class ExtractApkFile
    {
        public static void ExtractFileAndSave(string APKFilePath, string fileResourceLocation, string filePathToSave, int index)
        {
            using (var zip = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(APKFilePath)))
            {
                using (var fileStream = new FileStream(APKFilePath, FileMode.Open, FileAccess.Read))
                {
                    var zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(fileStream);
                    ICSharpCode.SharpZipLib.Zip.ZipEntry item;
                    
                    while ((item = zip.GetNextEntry()) != null)
                    {
                        if (item.Name.ToLower() == fileResourceLocation)
                        {
                            var fileLocation = Path.Combine(filePathToSave,
                                $"{index}-{fileResourceLocation.Split(Convert.ToChar(@"/")).Last()}");
                            using (var inputStream = zipFile.GetInputStream(item))
                            {
                                using (var output = File.Create(fileLocation))
                                {
                                    try
                                    {
                                        inputStream.CopyTo(output);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}