using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Collections;
using Sirenix.OdinInspector;

/// <summary>
/// 実行中のセッションを追跡し、記録する人
/// </summary>
public class SessionTracker : MonoBehaviour, IEventListener
{
    [SerializeField] AssetLabelReference L_sessionStartEvent;
    [SerializeField] AssetLabelReference L_sessionEndEvent;
    [SerializeField] AssetLabelReference L_realtimeEvent;
    [SerializeField] AssetLabelReference L_LastSessionDataVar;

    //Start又はEndのEvent
    SalvageEvent startEndEvent;
    SalvageEvent<ExploreArg> realtimeEvent;
    SalvageValuable<ISalvageData> lastSessionDataVar;

    [ShowInInspector,ReadOnly]
    SessionData lastSessionData
    {
        get
        {
            if (lastSessionDataVar == null)
            {
                return null;
            }
            return lastSessionDataVar.value as SessionData;
        }
        set
        {
            lastSessionDataVar.value = value;
        }
    }


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
    public bool OnNotice()
    {
        StartCoroutine(TransitionProcess());
        return true;
    }

    //状態遷移プロセス。
    //要するに出口または入り口の処理
    IEnumerator TransitionProcess()
    {
        startEndEvent.DisRegister(this);
        DataManager.ReleaseData(startEndEvent);
        //Sessionが動いていないとき
        if (lastSessionData == null || lastSessionData.isFinished)
        {
            var realtimeLoad = DataManager.LoadDataAsync(L_realtimeEvent);
            yield return new WaitUntil(() => realtimeLoad.ready);
            realtimeEvent = realtimeLoad.result as SalvageEvent<ExploreArg>;

            lastSessionData = new SessionData();
            realtimeEvent.Register(lastSessionData);

            var endTask = DataManager.LoadDataAsync(L_sessionEndEvent);
            yield return new WaitUntil(() => endTask.ready);
            startEndEvent = endTask.result as SalvageEvent;
        }
        else if (!lastSessionData.isFinished)
        {
            lastSessionData.Compleated();
            realtimeEvent.DisRegister(lastSessionData);

            DataManager.ReleaseData(realtimeEvent);

            var startTask = DataManager.LoadDataAsync(L_sessionStartEvent);
            yield return new WaitUntil(() => startTask.ready);
            startEndEvent = startTask.result as SalvageEvent;
        }
        else
        {
            Debug.LogError("SomethingWentWrong");
        }

        startEndEvent.Register(this);
    }


    //初期化
    IEnumerator InitEvents(SmallTask task = null)
    {
        var LSTask = DataManager.LoadDataAsync(L_LastSessionDataVar);
        yield return new WaitUntil(() => LSTask.ready);
        lastSessionDataVar = LSTask.result as SalvageValuable<ISalvageData>;

        var startTask = DataManager.LoadDataAsync(L_sessionStartEvent);
        yield return new WaitUntil(() => startTask.ready);
        startEndEvent = startTask.result as SalvageEvent;
        startEndEvent.Register(this);
    }




    void OnDisable()
    {
        startEndEvent.DisRegister(this);
        DataManager.ReleaseData(startEndEvent);

        if (lastSessionDataVar == null)
        {
            DataManager.ReleaseData(lastSessionDataVar);
        }
    }

}