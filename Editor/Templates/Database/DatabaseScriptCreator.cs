#if UNITY_EDITOR
using System.IO;
using UnityEditor;

namespace Krolti.DatabaseSO.Editor
{
    internal class DatabaseScriptCreator : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            string className = Path.GetFileNameWithoutExtension(pathName);

            string scriptText =
$@"
using UnityEngine;
using Krolti.DatabaseSO;

[CreateAssetMenu(fileName = ""{className}"", menuName = ""Database/{className}"")]
public class {className} : Database<{className}Data>
{{

}}
[System.Serializable]
public class {className}Data : DatabaseItem
{{

}}

";

            File.WriteAllText(pathName, scriptText);

            AssetDatabase.ImportAsset(pathName);
        }
    }
}
#endif