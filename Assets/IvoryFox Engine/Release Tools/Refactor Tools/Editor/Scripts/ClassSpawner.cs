using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityRandom = UnityEngine.Random;

namespace IvoryFox_Engine.Release_Tools.Refactor_Tools.Editor.Scripts
{
    public class ClassSpawner
    {
        private string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
        private string[] vowels = { "a", "e", "i", "o", "u" };
    
        public string pattern =
            "using System.Collections;\n " +
            "using System.Collections.Generic;\n" +
            "namespace {0} {{" +
            "using UnityEngine; \n" +
            "public class {1} {2}{{ \n" +
            "\t void Start() {{ }} \n" +
            "\t void Update() {{ }} \n" +
            "}}";

        public string pat = 
            @"using UnityEngine;{3}

namespace {0}
{{
	public class {1} {2}{{ 
        void Start() {{}} 
	    void Update() {{}} 
	}}
}}";
    
        private List<string> namespaces = new List<string>();
    
        private Dictionary<string,string> classesPath = new Dictionary<string, string>(){{"MonoBehaviour",""}};
    
        private List<string> allWords = new List<string>();
        public void Spawn(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                string class_namespase = string.Empty;
                string class_name = WordFinder2(UnityEngine.Random.Range(4, 10), true);
                string inheritBy = String.Empty;
                string importBy = String.Empty;
        
                string assetsPath = Directory.GetCurrentDirectory() + @"\Assets\";
                string scriptPath = @"Scripts\";
                string fileName = $"{class_name}.cs";
            
            
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    KeyValuePair<string, string> im = classesPath.ElementAt(UnityRandom.Range(0, classesPath.Count));
                    class_namespase = !string.IsNullOrEmpty(im.Value) ? $"{im.Value}" : WordFinder2(UnityEngine.Random.Range(4, 10), true);
                }
                else
                {
                    class_namespase = WordFinder2(UnityEngine.Random.Range(4, 10), true);
                }
            
                if (UnityRandom.Range(0, 3) > 0)
                {
                    KeyValuePair<string, string> im = classesPath.ElementAt(UnityRandom.Range(0, classesPath.Count));
                
                    inheritBy = $": {im.Key}";
                    if (!string.IsNullOrEmpty(im.Value))
                    {
                        importBy = $"\nusing {im.Value};";
                    }
                }
            
                classesPath.Add(class_name, class_namespase);
            
                string samplesPath = $"{class_namespase}" + @"\";
                if (!Directory.Exists(assetsPath + scriptPath + samplesPath))
                {
                    Directory.CreateDirectory(assetsPath + scriptPath + samplesPath);
                }

                string formattedFile = string.Format(pat, class_namespase, class_name, inheritBy, importBy);
                using (FileStream fs = File.Create(assetsPath + scriptPath + samplesPath + fileName))     
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes(formattedFile);    
                    fs.Write(title, 0, title.Length);
                }
            
            }
        }
    
        public string WordFinder2(int requestedLength, bool firstToUpper = false)
        {
            string word = "";

            if (requestedLength == 1)
            {
                word = GetRandomLetter(vowels);
            }
            else
            {
                for (int i = 0; i < requestedLength; i+=2)
                {
                    word += GetRandomLetter(consonants) + GetRandomLetter(vowels);
                }

                word = word.Replace("q", "qu").Substring(0, requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
            }

            if (firstToUpper)
            {
                word = char.ToUpper(word[0]) + word.Substring(1);
            }
        
            allWords.Add(word);
        
            return word;
        }

        private static string GetRandomLetter(string[] letters)
        {
            return letters[UnityRandom.Range(0, letters.Length - 1)];
        }
    }
}
