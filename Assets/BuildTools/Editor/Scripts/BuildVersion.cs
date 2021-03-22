using System;

namespace BuildTools.Editor.Scripts
{
    [Serializable]
    public class BuildVersion
    {
        public int currentNumber = 0;
        public string GetBuildVersionAsString() => $"build_{currentNumber}";
        public string GetNextBuildVersionAsString() => $"build_{currentNumber + 1}";
        public void Increase() => currentNumber++;
    }
}
