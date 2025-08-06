#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Krolti.DatabaseSO.Editor
{
    public static class DatabaseMenu
    {

        [MenuItem("Assets/Create/Database SO/Database Script", priority = -100)]
        public static void CreateDatabaseScript()
        {
            var creator = ScriptableObject.CreateInstance<DatabaseScriptCreator>();

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                creator,
                "NewDatabase.cs",
                TemplateTools.GetScriptIcon(),
                null
            );
        }
    }
}



#endif