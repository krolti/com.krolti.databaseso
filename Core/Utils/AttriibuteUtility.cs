using System;
using UnityEngine;

namespace Krolti.DatabaseSO
{
    /// <summary>
    /// Class to check generics for containing an attribute.
    /// </summary>
    public static class AttributeUtility
    {

        /// <summary>
        /// Main checking method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CheckDatabaseTypes<T>() where T : class, IDatabaseItem
        {
            if (!Application.isEditor) return;

            CheckSerializableAttribute<T>();
        }


        public static void CheckSerializableAttribute<T>() where T : class, IDatabaseItem
        {
            if (!IsSerializable<T>())
            {
                Debug.LogErrorFormat("[DATABASE_SO] Type '{0}' does not contain " +
                    "[System.Serializable] attribute, add it to work with Database",
                    typeof(T));
            }
        }

        private static bool IsSerializable<T>() => IsSerializable(typeof(T));
        private static bool IsSerializable(Type type) => type.IsDefined(typeof(SerializableAttribute), false);
    }
}
