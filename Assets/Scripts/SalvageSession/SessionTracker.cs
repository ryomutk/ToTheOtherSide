using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// 実行中のセッションを追跡し、記録する人
/// </summary>
public class SessionTracker : IEventListener<SessionEventArg>
{
    //List<SessionData> _ongoingSessions = new List<SessionData>();
    //Idとのセットで保持する。Idはsessionのではなく、Masterの。
    Dictionary<string, SessionData> _ongoingSessionTable = new Dictionary<string, SessionData>();
    LocationTracker locationTracker;

    public SessionTracker()
    {
        locationTracker = new LocationTracker(this);
    }


    //StartEv か EndEvが発行されたとき
    public ITask OnNotice(SessionEventArg arg)
    {
        return TransitionProcess(arg);
    }

    class LocationTracker : IEventListener<ExploreArg>
    {
        //Vec2intで大まかなオブジェクトの位置を保存。
        //#でのアクセスを可能にする。
        public Dictionary<Vector2Int, List<SessionData>> locationTable = new Dictionary<Vector2Int, List<SessionData>>();

        SessionTracker tracker;
        public LocationTracker(SessionTracker tracker)
        {
            EventManager.instance.Register(this, EventName.RealtimeExploreEvent);
        }
        public ITask OnNotice(ExploreArg arg)
        {
            if (arg.type == ExploreObjType.Travel)
            {
                var targ = arg as TravelExArg;
                var session = tracker._ongoingSessionTable[targ.from.id];
                var last = Vector2Int.FloorToInt(targ.coordinate - targ.traveledVec);
                var targetSessions = locationTable[last];
                if (targetSessions.Count == 1)
                {
                    targetSessions.Clear();
                }
                else if (targetSessions.Count > 1)
                {
                    for (int i = 0; i < targetSessions.Count; i++)
                    {
                        if (targetSessions[i].master.id == arg.from.id)
                        {
                            targetSessions.Remove(targetSessions[i]);
                            break;
                        }
                    }
                }
                else
                {
#if DEBUG
                    Debug.LogError("Something went wrong!!");
#endif
                }

                var now = Vector2Int.FloorToInt(targ.coordinate);
                if (locationTable[now] == null)
                {
                    locationTable[now] = new List<SessionData>();
                }

                locationTable[now].Add(session);

            }
            return SmallTask.nullTask;
        }


        ~LocationTracker()
        {
            EventManager.instance.Disregister(this, EventName.RealtimeExploreEvent);
        }
    }

    //状態遷移プロセス。
    //要するに出口または入り口の処理
    ITask TransitionProcess(SessionEventArg arg)
    {

        //Sessionが動いていないとき
        if (arg.state == SessionState.start)
        {
            var newSession = new SessionData(arg.data.master, arg.data.startCoordinate);

            //1機体複数セッションはここでのみ対応していない。
            //現状そんなことはないはずで、それにコストが見合わないため、とりあえずこのままにする
            _ongoingSessionTable[arg.data.master.id] = newSession;
            var realtimeLoad = EventManager.instance.Register(newSession, EventName.RealtimeExploreEvent);
            return realtimeLoad;
        }
        else if (arg.state == SessionState.compleate)
        {
            var targetSession = _ongoingSessionTable[arg.data.master.id];
            targetSession.Compleated();
            _ongoingSessionTable.Remove(arg.data.master.id);
            EventManager.instance.Disregister(targetSession, EventName.RealtimeExploreEvent);
        }

        return SmallTask.nullTask;
    }
}