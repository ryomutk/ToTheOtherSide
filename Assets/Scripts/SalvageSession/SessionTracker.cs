using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Collections;
using Sirenix.OdinInspector;

/// <summary>
/// 実行中のセッションを追跡し、記録する人
/// </summary>
public class SessionTracker :IEventListener<SessionEventArg>
{
    List<SessionData> ongoingSessions = new List<SessionData>();



    //StartEv か EndEvが発行されたとき
    public ITask OnNotice(SessionEventArg arg)
    {
        return TransitionProcess(arg);
    }

    //状態遷移プロセス。
    //要するに出口または入り口の処理
    ITask TransitionProcess(SessionEventArg arg)
    {

        //Sessionが動いていないとき
        if (arg.state == SessionState.start)
        {
            var newSession = new SessionData(arg.data.master);
            ongoingSessions.Add(newSession);
            var realtimeLoad = EventManager.instance.Register(newSession, EventName.RealtimeExploreEvent);
            return realtimeLoad;
        }
        else if (arg.state == SessionState.compleate)
        {
            var targetSession = ongoingSessions.Find(x => x.master == arg.data.master);
            targetSession.Compleated();
            ongoingSessions.Remove(targetSession);
            EventManager.instance.Disregister(targetSession, EventName.RealtimeExploreEvent);
        }

        return SmallTask.nullTask;
    }
}