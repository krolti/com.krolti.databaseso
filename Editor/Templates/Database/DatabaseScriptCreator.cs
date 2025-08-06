#if UNITY_EDITOR

using System.IO;
using UnityEngine;

namespace Krolti.DatabaseSO.Editor
{
    internal class DatabaseScriptCreator : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            string className = TemplateTools.SanitizeClassName
                (Path.GetFileNameWithoutExtension(pathName));

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

            TemplateTools.WriteFile(pathName, scriptText, "DatabaseScriptCreator");
            ScriptableObject.DestroyImmediate(this);
        }
    }
}

#endif