using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Collections;
using Sirenix.OdinInspector;

/// <summary>
/// 実行中のセッションを追跡し、記録する人
/// </summary>
public class SessionTracker : MonoBehaviour, IEventListener<SessionEventArg>
{
    List<SessionData> ongoingSessions = new List<SessionData>();



    void Start()
    {
        GameManager.instance.OnSystemEvent += (x) =>
        {
            var task = new SmallTask();
            StartCoroutine(InitEvents(task));

            return task;
        };
    }


    //StartEv か EndEvが発行されたとき
    public bool OnNotice(SessionEventArg arg)
    {
        StartCoroutine(TransitionProcess(arg));
        return true;
    }

    //状態遷移プロセス。
    //要するに出口または入り口の処理
    IEnumerator TransitionProcess(SessionEventArg arg)
    {

        //Sessionが動いていないとき
        if (arg.state == SessionState.start)
        {
            var newSession = new SessionData(arg.data.master);
            ongoingSessions.Add(newSession);
            var realtimeLoad = EventManager.instance.Register(newSession,EventName.RealtimeExploreEvent);
            yield return new WaitUntil(() => realtimeLoad.compleated);
        }
        else if (arg.state == SessionState.compleate)
        {
            var targetSession = ongoingSessions.Find(x => x.master == arg.data.master);
            targetSession.Compleated();
            ongoingSessions.Remove(targetSession);
            EventManager.instance.Disregister(targetSession,EventName.RealtimeExploreEvent);
        }

    }


    //初期化
    IEnumerator InitEvents(SmallTask task)
    {
        /*
        var LSTask = DataManager.LoadDataAsync(L_LastSessionDataVar);
        yield return new WaitUntil(() => LSTask.ready);
        lastSessionDataVar = LSTask.result as SalvageValuable<ISalvageData>;
        */

        var loadTask = EventManager.instance.Register(this,EventName.SessionEvent);
        yield return new WaitUntil(()=>loadTask.compleated);
        task.compleated = true;
    }




    void OnDisable()
    {
        EventManager.instance.Disregister(this,EventName.SessionEvent);
    }

}