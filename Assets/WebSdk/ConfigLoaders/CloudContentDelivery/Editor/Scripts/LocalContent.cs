using System;
using System.Linq;

namespace CloudContentDelivery.Editor.Scripts
{
    public class LocalContent
    {
        public string GetPathInCloud { get; }
        public string GetFilePath { get; }

        public LocalContent(string getFilePath)
        {
            GetFilePath = getFilePath;
            
            string pathWithExtension = getFilePath.Split(new[] { "Assets/" }, StringSplitOptions.None)[1]
                .Split('\\')
                .Last();
            
            GetPathInCloud = pathWithExtension;
        }
    }
}