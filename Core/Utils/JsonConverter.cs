using System;
using System.Collections.Generic;
using UnityEngine;

namespace Krolti.DatabaseSO
{
    public static class JsonConverter
    {
        public const int MAX_SAFE_SIZE_DESKTOP = 50 * 1024 * 1024;
        public const int MAX_SAFE_SIZE_MOBILE = 5 * 1024 * 1024;

        public static string ConvertToJson<T>(List<T> Data, bool prettyPrint) where T : class, IDatabaseItem
        {
            try
            {
                string json = JsonUtility.ToJson(new DatabaseWrapper<T>(Data), prettyPrint);
                return json;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogErrorFormat("[{0}] Failed to convert database to json. Type: {1}",
                    nameof(Database<T>),
                    typeof(T)
                    );
                UnityEngine.Debug.LogException(ex);
            }
            return string.Empty;
        }

        public static void ImportFromJSON<T>(string json, ref List<T> Data, int maxSaveSize = -1) where T : class, IDatabaseItem
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new System.ArgumentException($"[{nameof(Database<T>)}] " +
                    $"Null or empty string requested");
            }

            int maxSize = Application.isMobilePlatform ?
                MAX_SAFE_SIZE_MOBILE :
                MAX_SAFE_SIZE_DESKTOP;

            if(maxSaveSize > maxSize)
                maxSize = maxSaveSize;

            if (json.Length > maxSize)
            {
                throw new InvalidOperationException($"Save data too large ({json.Length} bytes). " +
                    $"Max allowed: {maxSize} bytes, increase via maxSaveSize overload");
            }

            try
            {
                var wrapper = JsonUtility.FromJson<DatabaseWrapper<T>>(json);
                Data = wrapper.Items;

            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogErrorFormat("[{0}] Failed to read json data. Type: {1}",
                    nameof(Database<T>),
                    typeof(T)
                    );
                UnityEngine.Debug.LogException(ex);
            }
        }
    }
}
