using System;
using System.Collections.Generic;
using UnityEngine;

namespace Krolti.DatabaseSO
{
    internal static class JsonConverter
    {
        private const int MAX_SAFE_SIZE_DESKTOP = 50 * 1024 * 1024;
        private const int MAX_SAFE_SIZE_MOBILE = 5 * 1024 * 1024;

        public static string ConvertToJson<T>(List<T> Data, bool prettyPrint) where T : class, IDatabaseItem
        {
            try
            {
                string json = JsonUtility.ToJson(new DatabaseWrapper<T>(Data), prettyPrint);
                return json;
            }
            catch (Exception ex)
            {
                DebugDB.Error<T>("Failed to convert database to json.");
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
                DebugDB.Error<T>("Failed to read json data.");
                UnityEngine.Debug.LogException(ex);
            }
        }
    }
}
