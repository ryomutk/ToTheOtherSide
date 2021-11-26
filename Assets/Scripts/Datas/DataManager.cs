using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 各種データを集積し、DataManagerに渡す人
/// </summary>
/// <typeparam name="T"></typeparam>
public static class DataManager
{
    public static ITask<DataIndexer> LoadDatasAsync(AssetLabelReference label)
    {

        var loadTask = Addressables.LoadAssetsAsync<ISalvageData>(label.labelString, (x) => { });

        var task = new SmallTask<DataIndexer>(
            () => loadTask.IsDone,
            () =>
            {
                return new DataIndexer((List<ISalvageData>)loadTask.Result, true);
            });

        return task;
    }

    public static ITask<ISalvageData> LoadDataAsync(AssetLabelReference label)
    {
        var loadTask = Addressables.LoadAssetAsync<ISalvageData>(label.labelString);

        var task = new SmallTask<ISalvageData>(
            () => loadTask.IsDone,
            () =>
            {
                return loadTask.Result;
            });


        return task;
    }

    /*
        static IEnumerator FetchData(string label, SmallTask<DataIndexer> task)
        {
            var loadTask = Addressables.LoadAssetsAsync<ISalvageData>(label, (x) => { });

            yield return new WaitUntil(() => loadTask.IsDone);

            var result = new DataIndexer((List<ISalvageData>)loadTask.Result, true);

            task.result = result;
        }
    */
    static public void ReleaseDatas(DataIndexer datas)
    {
        /*
        foreach (var data in datas)
        {
            Addressables.Release<ISalvageData>(data);
        }
        */
        Addressables.Release<List<ISalvageData>>(datas.entity);

        datas = null;
    }

    static public void ReleaseData(ISalvageData data)
    {
        Addressables.Release<ISalvageData>(data);
        data = null;
    }
}