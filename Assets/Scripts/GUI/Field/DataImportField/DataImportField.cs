using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AddressableAssets;

//DataManagerからkeyのデータを読み込み、
//指定された場所に配分する
public abstract class DataImportField : MonoBehaviour,IInformationField
{
    protected abstract AssetLabelReference loadKey{get;}

    //public abstract Func<bool> LoadData();
    protected DataIndexer entity;

    //LoadDataをオーバーライドして択の変数を読み込む
    public SmallTask LoadDataAsync()
    {
        var task = new SmallTask();
        StartCoroutine(LoadRoutine(task));

        return task;
    }


    protected virtual IEnumerator LoadRoutine(SmallTask task)
    {
        var loadTask = DataManager.LoadDatasAsync(loadKey);
        yield return new WaitUntil(()=>loadTask.ready);

        entity = loadTask.result;
        task.ready = true;

        SetField();
    }

    protected abstract void SetField();

    public virtual void UnloadData()
    {
        DataManager.ReleaseDatas(entity);
    }
}