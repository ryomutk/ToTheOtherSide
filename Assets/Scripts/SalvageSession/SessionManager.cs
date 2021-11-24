using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Collections;
using Sirenix.OdinInspector;
using System.Text;

public class SessionManager : MonoBehaviour
{
    ISessionSequencer sequencer = null;



    public void SummarizeSession(ArmBotData.Entity entity)
    {
        StartCoroutine(SummaryRoutine(entity));
    }

    IEnumerator SummaryRoutine(ArmBotData.Entity sessionMaster)
    {

        var summaryData = new SessionData(sessionMaster);
        //EvBuilderとSequencerを準備
        ISessionSequencer sequencer = new SessionSequencerOnTheCliff();

        var eventRegister = EventManager.instance.Register(summaryData, EventName.SystemExploreEvent);
        yield return new WaitUntil(() => eventRegister.ready);
        //処理開始
        sequencer.BuildSession();



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

        summaryData.Compleated();
        EventManager.instance.Disregister(summaryData, EventName.SystemExploreEvent);

        //サマリーを通知
        EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.summary, summaryData));

        summarizedData = summaryData;
    }

    SessionData summarizedData = null;
    public void StartSummarizedSession()
    {
        if (summarizedData != null)
        {
            var session = EventManager.instance.Notice(EventName.SessionEvent, new SessionEventArg(SessionState.start, summarizedData));

            StartCoroutine(RealtimeSessionRoutine(summarizedData));
            summarizedData = null;
        }
        else
        {
            Debug.LogWarning("Something went wrong");
        }
    }

    //本作のメインコンテンツ
    IEnumerator RealtimeSessionRoutine(SessionData sessionData)
    {
        var stepClock = 0;

        foreach (var events in sessionData.eventsOccoured)
        {
            throw new System.NotImplementedException();

            yield return new WaitForSeconds(SessionConfig.instance.durationPerStep);
            stepClock++;
        }

        //結果を報告
        var resEvent = DataManager.LoadDataAsync(L_sessionEvent);
        yield return new WaitUntil(() => resEvent.ready);

        throw new System.NotImplementedException();
        //var sessionArg = new SessionEventArg(SessionState.compleate,SessionData);

        //(resEvent.result as SalvageEvent<SessionEventArg>).Notice(sessionArg);

        DataManager.ReleaseData(resEvent.result);
    }


}