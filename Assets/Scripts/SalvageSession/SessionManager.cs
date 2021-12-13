using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using System;
using System.Text;

//ドローンの飛行セッション管理を行う
public class SessionManager : MonoBehaviour, IEventListener<SessionEventArg>
{
    SessionTracker tracker;



    void Start()
    {
        tracker = new SessionTracker();
        EventManager.instance.Register(tracker, EventName.SessionEvent);
        EventManager.instance.Register(this, EventName.SessionEvent);
    }

    //Taskに持たせるやつ。
    //このクラス内だけで書き換えれるようにするためにこうした。
    class DataContainer
    {
        public bool result { get { return data != null; } }
        public SessionData data = null;
    }

    //リクエストベースで動かすことにしました。
    public ITask OnNotice(SessionEventArg arg)
    {
        if (arg.state == SessionState.requestSummary)
        {
            return RequestSummary(arg.data.master, arg.data.startCoordinate);
        }
        else if (arg.state == SessionState.requestSession)
        {
            StartCoroutine(RealtimeSessionRoutine(arg.data));
        }
        return SmallTask.nullTask;
    }


    /// <summary>
    /// 使うEntity,発射場所、方向を投げると、Sessionサマリーを構築する。
    /// </summary>
    /// <param name="entity">セッションマスターさん</param>
    /// <param name="startCords">発射場所</param>
    /// <returns>構築のタスク</returns>
    ITask<SessionData> RequestSummary(ArmBotData.Entity entity, Vector2 startCords)
    {
        DataContainer container = new DataContainer();

        var task = new SmallTask<SessionData>(
            () => container.result, () => container.data
        );

        StartCoroutine(SummaryRoutine(entity, container, startCords));

        return task;
    }

    //システムSessionEventを発行してくれるサマリーシーケンス。
    //これの結果はSessionDataに書き込まれ,タスクを介してリクエスト元に返される.
    IEnumerator SummaryRoutine(ArmBotData.Entity sessionMaster, DataContainer container, Vector2 startCords)
    {
        var summaryGhost = sessionMaster.GetGhost();
        var summaryData = new SessionData(summaryGhost, startCords);

        //EvBuilderとSequencerを準備
        //ISessionSequencer sequencer = new SessionSequencerOnTheCliff(DataProvider.nowGameData.map, sessionMaster, startCords,summaryData);

        var eventRegister = EventManager.instance.Register(summaryData, EventName.SystemExploreEvent);
        yield return new WaitUntil(() => eventRegister.compleated);
        var map = DataProvider.nowGameData.map;
        //処理開始
        while (!summaryGhost.CheckIfEnd())
        {
            summaryGhost.OnUpdate(map, summaryData.nowCoordinate);
        }


        //今後EventManagerで諸Event のログをつけてもらうようにする
        /*
                //ログを全部書く
        #if UNITY_EDITOR
                var stringBuilder = new StringBuilder();

                foreach (var ev in summaryData.eventsOccoured)
                {
                    stringBuilder.Append("---");
                    ev.BuildLog(ref stringBuilder);
                }

                Utility.LogWriter.Log(stringBuilder.ToString(), "EventLog", false);
        #endif
        */

        summaryData.Compleated();
        EventManager.instance.Disregister(summaryData, EventName.SystemExploreEvent);

        //サマリーを通知
        EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.summary, summaryData));

        container.data = summaryData;
    }


    //本作のメインコンテンツ。RealtimeExploreEventを発行するシーケンスを作る。
    IEnumerator RealtimeSessionRoutine(SessionData sessionData)
    {
        var started = EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.start, sessionData));
        yield return new WaitUntil(() => started.compleated);
        var session = tracker.ongoingSessionTable[sessionData.master.id];

        bool updated = false;
        Action onStep = () =>
        {
            updated = true;
        };

        session.onUpdate += onStep;

        while (session.master.CheckIfEnd())
        {
            session.master.OnUpdate(DataProvider.nowGameData.map, session.nowCoordinate);
            yield return new WaitUntil(() => updated);
            updated = false;
            var arg = session.eventsOccoured[session.eventsOccoured.Count-1];
            var time = SessionConfig.instance.GetDuration(arg);
            if(time!=0){yield return new WaitForSeconds(time);}
        }

        session.onUpdate -= onStep;

        /*
        var stepClock = 0;
        foreach (var exArg in sessionData.eventsOccoured)
        {
            var duration = SessionConfig.instance.GetDuration(exArg);
            yield return new WaitForSeconds(duration);
            var stepTask = EventManager.instance.Notice<ExploreArg>(EventName.RealtimeExploreEvent, exArg);

            //処理おおきにつき、ちゃんと確認
            if (!stepTask.compleated)
            {
                yield return new WaitUntil(() => stepTask.compleated);
            }

            stepClock++;
        }
        */

        //終了を報告
        EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.compleate, sessionData));
    }


    void OnDisable()
    {
        EventManager.instance.Disregister(tracker, EventName.SessionEvent);
    }

}