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

    public Dictionary<string,SessionData> ongoingSessionTable{get{return _ongoingSessionTable;}}

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
            this.tracker = tracker;
            EventManager.instance.Register(this, EventName.RealtimeExploreEvent);
        }
        public void RegisterSession(SessionData data)
        {
            var start = Vector2Int.FloorToInt(data.startCoordinate);

            if(!locationTable.ContainsKey(start))
            {
                locationTable[start] = new List<SessionData>();
            }
            locationTable[start].Add(data);
        }
        public void DisregisterSession(SessionData data)
        {
            var end = Vector2Int.FloorToInt(data.nowCoordinate);
            var result = locationTable[end].Remove(data);
#if DEBUG
            if (!result)
            {
                Debug.LogError("Session not found!");
            }
#endif
        }
        public ITask OnNotice(ExploreArg arg)
        {
            if (arg.type == ExploreObjType.Travel)
            {
                var targ = arg as TravelExArg;
                var session = tracker._ongoingSessionTable[targ.from.id];
                var last = Vector2Int.FloorToInt(session.nowCoordinate - targ.traveledVec);
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

                var now = Vector2Int.FloorToInt(session.nowCoordinate);
                if (!locationTable.ContainsKey(now))
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
            SessionData newSession = null;
            if(arg.data.master.isGhost)
            {
                newSession = new SessionData(arg.data.master.realBody, arg.data.startCoordinate);
            }
            else
            {
                Debug.LogWarning("why are you alive?");
                newSession = new SessionData(arg.data.master,arg.data.startCoordinate);
            }

            //1機体複数セッションはここでのみ対応していない。
            //現状そんなことはないはずで、それにコストが見合わないため、とりあえずこのままにする
            _ongoingSessionTable[newSession.master.id] = newSession;
            var realtimeLoad = EventManager.instance.Register(newSession, EventName.RealtimeExploreEvent);

            locationTracker.RegisterSession(newSession);

            return realtimeLoad;
        }
        else if (arg.state == SessionState.compleate)
        {
            var targetSession = _ongoingSessionTable[arg.data.master.id];
            targetSession.Compleated();
            locationTracker.DisregisterSession(arg.data);
            _ongoingSessionTable.Remove(arg.data.master.id);
            EventManager.instance.Disregister(targetSession, EventName.RealtimeExploreEvent);
        }

        return SmallTask.nullTask;
    }
}