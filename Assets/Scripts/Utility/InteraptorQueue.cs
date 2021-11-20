using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteraptorQueue
{
    MonoBehaviour parent;
    public bool working { get; private set; }
    List<SmallTask> interaptors = new List<SmallTask>();

    /// <summary>
    /// parentはこれを実装するMonobehaviour。
    /// Ienumeratorの実行に必要
    /// </summary>
    /// <param name="parent"></param>
    public InteraptorQueue(MonoBehaviour parent)
    {
        this.parent = parent;
        working = false;
    }

    public void RegisterQueue(SmallTask interaptor)
    {
        interaptors.Add(interaptor);
    }

    /// <summary>
    /// Queueを処理します
    /// </summary>
    /// <returns>処理したInteraptorの数</returns>
    public int SolveQueue()
    {
        if(interaptors.Count!=0)
        {
            working = true;
            parent.StartCoroutine(HandleQueue());
        }

        return interaptors.Count;
    }

    IEnumerator HandleQueue()
    {
        while (interaptors.Count != 0)
        {
            var task = interaptors[0];
            yield return new WaitUntil(() =>task.ready);
            interaptors.RemoveAt(0);
        }

        working = false;
    }
}