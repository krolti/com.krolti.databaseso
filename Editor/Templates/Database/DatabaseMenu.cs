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
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<DatabaseScriptCreator>(),
                "NewDatabase.cs",
                null,
                null
            );
        }
    }
}



#endif