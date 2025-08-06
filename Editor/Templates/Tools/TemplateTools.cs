#if UNITY_EDITOR

using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Krolti.DatabaseSO.Editor
{
    public static class TemplateTools 
    {
        public static string SanitizeClassName(string rawName)
        {
            return Regex.Replace(rawName, @"[^a-zA-Z0-9_]", "_");
        }


        public static void WriteFile(string pathName, string scriptText, string errorContext)
        {
            try
            {
                File.WriteAllText(pathName, scriptText);
                AssetDatabase.ImportAsset(pathName);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[{errorContext}] Editor error during file write");
                Debug.LogException(ex);
            }
        }

        public static Texture2D GetScriptIcon()
        {
            return EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
        }
    }
}
#endif