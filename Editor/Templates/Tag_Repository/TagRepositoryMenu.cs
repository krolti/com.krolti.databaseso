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
            var creator = ScriptableObject.CreateInstance<TagRepositoryScriptCreator>();

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