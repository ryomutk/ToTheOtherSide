using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using System;
using System.Text;

public class SessionManager : MonoBehaviour
{
    ISessionSequencer sequencer = null;
    SessionTracker tracker = new SessionTracker();


    void Start()
    {
        EventManager.instance.Register(tracker, EventName.SessionEvent);
    }

    //Taskに持たせるやつ。
    //このクラス内だけで書き換えれるようにするためにこうした。
    class DataContainer
    {
        public bool result { get { return data != null; } }
        public SessionData data = null;
    }


    /// <summary>
    /// 使うEntity,発射場所、方向を投げると、Sessionサマリーを構築する。
    /// </summary>
    /// <param name="entity">セッションマスターさん</param>
    /// <param name="startCords">発射場所</param>
    /// <returns>構築のタスク</returns>
    public ITask<SessionData> RequestSummary(ArmBotData.Entity entity,Vector2Int startCords)
    {
        DataContainer container = new DataContainer();

        var task = new SmallTask<SessionData>(
            () => container.result, () => container.data
        );

        StartCoroutine(SummaryRoutine(entity, container,startCords));

        return task;
    }

    //システムSessionEventを発行してくれるサマリーシーケンス。
    //これの結果はSessionDataに書き込まれ,タスクを介してリクエスト元に返される.
    IEnumerator SummaryRoutine(ArmBotData.Entity sessionMaster, DataContainer container,Vector2Int startCords)
    {

        var summaryData = new SessionData(sessionMaster);

        //EvBuilderとSequencerを準備
        ISessionSequencer sequencer = new SessionSequencerOnTheCliff(DataProvider.nowGameData.map ,sessionMaster,startCords);

        var eventRegister = EventManager.instance.Register(summaryData, EventName.SystemExploreEvent);
        yield return new WaitUntil(() => eventRegister.compleated);
        //処理開始
        sequencer.BuildSession();


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

    //リアルタイム探索セッションを開始する。
    //現在、Summaryで作ったやつを投げて、実行することを前提としているが、SessionDataさえ残しておけば、
    //リプレイなども実装できる。べんり～
    public void ExecuteSession(SessionData sessionData)
    {
        StartCoroutine(RealtimeSessionRoutine(sessionData));
    }

    //本作のメインコンテンツ。RealtimeExploreEventを発行するシーケンスを作る。
    IEnumerator RealtimeSessionRoutine(SessionData sessionData)
    {
        var started = EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.start, sessionData));
        yield return new WaitUntil(()=>started.compleated);


        var stepClock = 0;

        foreach (var exArg in sessionData.eventsOccoured)
        {
            var duration = SessionConfig.instance.GetDuration(exArg);
            yield return new WaitForSeconds(duration);
            var stepTask = EventManager.instance.Notice(EventName.RealtimeExploreEvent, exArg);


            //処理おおきにつき、ちゃんと確認
            if (!stepTask.compleated)
            {
                yield return new WaitUntil(() => stepTask.compleated);
            }

            stepClock++;
        }

        //終了を報告
        EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.compleate,sessionData));
    }


    void OnDisable()
    {
        EventManager.instance.Disregister(tracker, EventName.SessionEvent);
    }

}