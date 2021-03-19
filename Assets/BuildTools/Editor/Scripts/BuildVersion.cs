using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    public static class BuildVersion
    {
        private const string BuildNumberConst = "build_number";

        public static int CurrentNumber() => PlayerPrefs.GetInt(BuildNumberConst, 1);
        public static void IncreaseNumber() => PlayerPrefs.SetInt(BuildNumberConst, CurrentNumber() + 1);
        public static string GetBuildVersionAsString() => $"build_{CurrentNumber()}";
    }
}
