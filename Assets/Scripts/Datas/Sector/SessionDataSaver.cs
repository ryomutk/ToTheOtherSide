using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;

public class SessionDataSaver : MonoBehaviour, IEventListener<SessionEventArg>
{
    [SerializeField] AssetLabelReference L_sessionEv;
    [SerializeField] AssetLabelReference L_nowSessionData;
    [SerializeField] SerializableDictionary<SessionState, List<AssetLabelReference>> permanentDataLabels;
    SalvageEvent<SessionEventArg> sessionEvent;

    void Start()
    {
        GameManager.instance.OnSystemEvent += (x) =>
        {
            if (x == GameState.SystemInitialize)
            {
                var task = new SmallTask();
                StartCoroutine(DataLoadRoutine(task));
                return task;
            }

            return SmallTask.nullTask;
        };
    }

    IEnumerator DataLoadRoutine(SmallTask task)
    {
        var loadTask = DataManager.LoadDataAsync(L_sessionEv);
        yield return new WaitUntil(() => loadTask.compleated);

        sessionEvent = loadTask.result as SalvageEvent<SessionEventArg>;
        task.compleated = true;
    }

    public bool OnNotice(SessionEventArg arg)
    {
        if (permanentDataLabels.ContainsKey(arg.state))
        {
            StartCoroutine(SaveDataRoutine(arg.state));
            return true;
        }

        return false;
    }

    IEnumerator SaveDataRoutine(SessionState state)
    {
        var sDataLoadTask = DataManager.LoadDataAsync(L_nowSessionData);
        yield return new WaitUntil(()=>sDataLoadTask.compleated);

        if (permanentDataLabels.TryGetItem(state, out var labels))
        {
            foreach (var label in labels)
            {
                var pLoadTask = DataManager.LoadDataAsync(label);

                yield return new WaitUntil(() => pLoadTask.compleated);

                ((IPermanentData)pLoadTask.result).UpdateData(sDataLoadTask.result);

                DataManager.ReleaseData(pLoadTask.result);
            }
        }

        DataManager.ReleaseData(sDataLoadTask.result);
    }


    void OnDisable()
    {
        //
        if (sessionEvent != null)
        {
            DataManager.ReleaseData(sessionEvent);
        }
    }

}