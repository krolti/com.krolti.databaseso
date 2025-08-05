#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Krolti.DatabaseSO.Editor
{
    public static class TagRepositoryMenu
    {
        [MenuItem("Assets/Create/Database SO/Tag Repository Script", priority = -90)]

        public static void CreateDatabaseScript()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<TagRepositoryScriptCreator>(),
                "NewDatabase.cs",
                null,
                null
            );
        }
    }
}
#endif