using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

#if UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Krolti.DatabaseSO.Examples
{
    public class BenchmarkUtility : MonoBehaviour
    {
        [SerializeField] private SimpleDatabase database;
        [SerializeField] private int searchIndex;

        private CancellationTokenSource _cts;
        private readonly Stopwatch _stopwatch = new();


        #region Benchmark
        public void BenchmarkSearch(int operationsCount)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            for (int i = 0; i < operationsCount; i++)
            {
                var value = database.Search(searchIndex);
                if(value == null)
                {
                    UnityEngine.Debug.LogWarning("Null item");
                    break;
                }
            }

            _stopwatch.Stop();
            UnityEngine.Debug.Log($"Elapsed time search: {_stopwatch.ElapsedMilliseconds}ms");
            _stopwatch.Reset();
        }



        public void BenchmarkFor(int operationsCount)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            for (int i = 0; i < operationsCount; i++)
            {
                database.Becnchmark_ForExample(searchIndex);
            }

            _stopwatch.Stop();
            UnityEngine.Debug.Log($"Elapsed time for: {_stopwatch.ElapsedMilliseconds}ms");
            _stopwatch.Reset();
        }



        public void BenchmarkForeach(int operationsCount)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            for (int i = 0; i < operationsCount; i++)
            {
                database.Becnchmark_ForeachExample(searchIndex);
            }

            _stopwatch.Stop();
            UnityEngine.Debug.Log($"Elapsed time for each: {_stopwatch.ElapsedMilliseconds}ms");
            _stopwatch.Reset();
        }



        public void BenchmarkContains(int operationsCount)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            for (int i = 0; i < operationsCount; i++)
            {
                database.ContainsID(searchIndex);
            }

            _stopwatch.Stop();
            UnityEngine.Debug.Log(database.ContainsID(searchIndex));
            UnityEngine.Debug.Log($"Elapsed time contains: {_stopwatch.ElapsedMilliseconds}ms");
            _stopwatch.Reset();
        }



        public void BenchmarkToJson(int operationsCount)
        {
            if(operationsCount > 20)
            {
                throw new System.ArgumentOutOfRangeException(nameof(operationsCount));
            }
            string json = string.Empty;
            _stopwatch.Reset();
            _stopwatch.Start();

            for (int i = 0; i < operationsCount; i++)
            {
                json = database.ConvertToJson();
            }

            _stopwatch.Stop();
            UnityEngine.Debug.Log($"Elapsed time to json: {_stopwatch.ElapsedMilliseconds}ms");
            UnityEngine.Debug.Log(json);
            _stopwatch.Reset();
        }
        #endregion

        #region Json async Export

        public void ExportToJsonAsync()
        {
#if UNITASK
            ExportToJsonUniTaskAsync();
#else
            StartExport();
#endif
        }



        public async void StartExport()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            if (database.Count == 0)
            {
                UnityEngine.Debug.LogWarning($"[{nameof(BenchmarkUtility)}] Database is empty");
                return;
            }

            try
            {
                string json = await database.ConvertToJsonAsync(true, _cts.Token);

                UnityEngine.Debug.Log($"[{nameof(BenchmarkUtility)}] Export successful! Length: {json.Length} chars");
                
                string path = Application.persistentDataPath + "/export.json";
                await System.IO.File.WriteAllTextAsync(path, json, _cts.Token);
                UnityEngine.Debug.Log($"Saved to: {path}");
            }
            catch (OperationCanceledException)
            {
                UnityEngine.Debug.Log($"[{nameof(BenchmarkUtility)}] Export was cancelled");
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e);
                UnityEngine.Debug.LogError($"[{nameof(BenchmarkUtility)}] Export failed: {e.Message}");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

#if UNITASK

        public void ExportToJsonUniTaskAsync()
        {
            StartExportUniTask().Forget();
        }

        public async UniTaskVoid StartExportUniTask()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            if (database.Count == 0)
            {
                UnityEngine.Debug.LogWarning($"[{nameof(BenchmarkUtility)}] Database is empty");
                return;
            }

            try
            {
                string json = await database.ConvertToJsonUniTaskAsync(true, _cts.Token);

                UnityEngine.Debug.Log($"[{nameof(BenchmarkUtility)}] UniTask Export successful! Length: {json.Length} chars");

                string path = Application.persistentDataPath + "/export_UniTask.json";
                await System.IO.File.WriteAllTextAsync(path, json, _cts.Token);
                UnityEngine.Debug.Log($"Saved to: {path}");
            }
            catch (OperationCanceledException)
            {
                UnityEngine.Debug.Log($"[{nameof(BenchmarkUtility)}] UniTask Export was cancelled");
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e);
                UnityEngine.Debug.LogError($"[{nameof(BenchmarkUtility)}] UniTask Export failed: {e.Message}");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }
#endif



        public void CancelExport()
        {
            _cts?.Cancel();
        }



        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
#endregion

    }
}
