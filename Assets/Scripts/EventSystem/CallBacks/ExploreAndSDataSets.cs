using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 投げられたExArgを判定して、対応するSalvageDataを保管しているDataクラス
/// Dataを直接受け取るのではなく、
/// GetEventListenerで
/// 該当するExArgを受けたときに返されるSalvageDataに対して行うコールバックを投げると、
/// そのコールバックを実行してくれるExEventのListenerを取得できる
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ExploreAndSDataSets<T> : ScriptableObject, ISalvageData
where T : ISalvageData
{
    //クローン元たち。
    //ランタイムでの値が変わると面倒なのでこいつらをそのままは使わない。
    protected abstract List<SalvagetimeExArgAndSDataset<T>> callbackDatas{get;}
    
    //寿命がSalvageセッション1回分のEventListener.
    public virtual IEventListener<ExploreArg> GetSalvageTimeEventListener(Action<T> callback)
    {
        return new SimpleExEventListener(callback, callbackDatas);
    }

    class SimpleExEventListener : IEventListener<ExploreArg>
    {
        Action<T> callback;
        List<SalvagetimeExArgAndSDataset<T>> dataSetList;
        List<T> results = new List<T>();

        public SimpleExEventListener(Action<T> resultSolver, List<SalvagetimeExArgAndSDataset<T>> datas)
        {
            dataSetList = new List<SalvagetimeExArgAndSDataset<T>>();
            foreach (var item in datas)
            {
                dataSetList.Add(item.Clone());
            }
            this.callback = resultSolver;
            dataSetList = datas;
        }

        public bool OnNotice(ExploreArg arg)
        {
            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    callback(results[i]);
                }
            }

            return results.Count > 0;
        }

        int TriggerAll(ExploreArg arg)
        {
            results.Clear();
            var count = 0;

            for (int i = 0; i < dataSetList.Count; i++)
            {
                var result = dataSetList[i].Trigger(arg);
                if (result != null)
                {
                    results.Add(result);
                    count++;
                }
            }

            return count;
        }

    }


}


public abstract class SalvagetimeExArgAndSDataset<M>
where M : ISalvageData
{
    public abstract SalvagetimeExArgAndSDataset<M> Clone();
    
    
    public abstract M Trigger(ExploreArg arg);
}