using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Collections;
using Sirenix.OdinInspector;
using System.Text;

public class SessionManager : MonoBehaviour
{
    [SerializeField] AssetLabelReference armBotDataLabel;
    [SerializeField] AssetLabelReference L_sectorData;
    [SerializeField] AssetLabelReference L_explore;
    [SerializeField] AssetLabelReference L_exEvent;
    [SerializeField] AssetLabelReference L_realtimeEv;
    [SerializeField] AssetLabelReference L_sessionEvent;
    ExploreEventBuilder builder = null;
    ISessionSequencer sequencer = null;
    


    [Button]
    void TryShot()
    {
        StartCoroutine(LoadRoutine());
    }

    IEnumerator LoadRoutine()
    {
        //必要なデータを準備
        var exData = DataManager.LoadDatasAsync(L_explore);
        yield return new WaitUntil(() => exData.ready);
        var botTask = DataManager.LoadDatasAsync(armBotDataLabel);
        yield return new WaitUntil(() => botTask.ready);
        var sectorTask = DataManager.LoadDatasAsync(L_sectorData);
        yield return new WaitUntil(() => sectorTask.ready);
        var exEvent = DataManager.LoadDatasAsync(L_exEvent);
        yield return new WaitUntil(() => exEvent.ready);

        //BotDataはEntityさえ手に入ればお役御免
        var entity = botTask.result.GetData<ArmBotData>(0).CreateInstance();
        DataManager.ReleaseDatas(botTask.result);


        //EvBuilderとSequencerを準備
        builder = new ExploreEventBuilder(exEvent.result.GetData<SalvageEvent<ExploreArg>>(0));
        ISessionSequencer sequencer = new SessionSequencerProto(exData.result.GetData<ExploreData>(0), sectorTask.result, entity, exEvent.result.GetData<SalvageEvent<ExploreArg>>(0));

        //処理開始
        sequencer.BuildSession();


        //終わったらデータ解放
        DataManager.ReleaseDatas(exData.result);
        DataManager.ReleaseDatas(sectorTask.result);
        DataManager.ReleaseDatas(exEvent.result);

//ログを全部書く
#if UNITY_EDITOR
        var stringBuilder = new StringBuilder();

        foreach (var ev in builder.timeline)
        {
            stringBuilder.Append("---");
            stringBuilder.AppendLine(ev.Key.ToString());
            for (int i = 0; i < ev.Value.Count; i++)
            {
                ev.Value[i].BuildLog(ref stringBuilder);
            }
        }

        Utility.LogWriter.Log(stringBuilder.ToString(), "EventLog", false);
#endif

        //諸々の処理を終えた後、Salvageの開始を伝える
        var sessionEventTask = DataManager.LoadDataAsync(L_sessionEvent);
        yield return new WaitUntil(() => sessionEventTask.ready);

        var summaryData = new SessionData();
        
        foreach (var ev in builder.allEvents)
        {
            //今回起こることすべてを読み取らせる。
            summaryData.OnNotice(ev);
        }
        //忘れない。
        summaryData.Compleated();

        //スタートを通知
        var sessionEvent = sessionEventTask.result as SalvageEvent<SessionEventArg>;
        var sessionArg = new SessionEventArg(SessionState.summary,summaryData);
        sessionEvent.Notice(sessionArg);

        //
        DataManager.ReleaseData(sessionEventTask.result);

        //ここにサマリーの終了を待つフェーズを加える。
        throw new System.NotImplementedException();

        //メインコンテンツ
        var realEventTask = DataManager.LoadDatasAsync(L_realtimeEv);
        yield return new WaitUntil(() => realEventTask.ready);
        yield return StartCoroutine(RealtimeSessionRoutine(realEventTask.result.GetData<SalvageEvent<ExploreArg>>(0)));
        DataManager.ReleaseDatas(realEventTask.result);
    }

    //本作のメインコンテンツ
    IEnumerator RealtimeSessionRoutine(SalvageEvent<ExploreArg> realTimeEvent)
    {
        var stepClock = 0;

        foreach (var events in builder.timeline)
        {
            if (events.Key == stepClock)
            {
                foreach (var item in events.Value)
                {
                    realTimeEvent.Notice(item);
                }
            }

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