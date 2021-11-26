using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;

public class SessionDataSaver : MonoBehaviour, IEventListener<SessionEventArg>, IEventListener<SystemEventArg>
{
    [SerializeField] AssetLabelReference L_nowSessionData;
    [SerializeField] SerializableDictionary<SessionState, List<AssetLabelReference>> permanentDataLabels;

    void Start()
    {
        EventManager.instance.Register<SystemEventArg>(this, EventName.SystemEvent);
    }

    IEnumerator DataLoadRoutine(SmallTask task)
    {
        var loadTask = EventManager.instance.Register<SessionEventArg>(this, EventName.SessionEvent);
        yield return new WaitUntil(() => loadTask.compleated);
        task.compleated = true;
    }

    public ITask OnNotice(SystemEventArg arg)
    {
        if (arg.state == GameState.SystemInitialize)
        {
            var task = new SmallTask();
            StartCoroutine(DataLoadRoutine(task));
            EventManager.instance.Disregister<SystemEventArg>(this, EventName.SystemEvent);
            return task;
        }

        return SmallTask.nullTask;
    }

    public ITask OnNotice(SessionEventArg arg)
    {
        if (permanentDataLabels.ContainsKey(arg.state))
        {
            StartCoroutine(SaveDataRoutine(arg.state));
        }

        return SmallTask.nullTask;
    }

    IEnumerator SaveDataRoutine(SessionState state)
    {
        var sDataLoadTask = DataManager.LoadDataAsync(L_nowSessionData);
        yield return new WaitUntil(() => sDataLoadTask.compleated);

        if (permanentDataLabels.TryGetItem(state, out var labels))
        {
            foreach (var label in labels)
            {
                var pLoadTask = DataManager.LoadDataAsync(label);

                yield return new WaitUntil(() => pLoadTask.compleated);

                ((IPermanentData)pLoadTask.result).UpdateData(sDataLoadTask.result as SessionData);

                DataManager.ReleaseData(pLoadTask.result);
            }
        }

        DataManager.ReleaseData(sDataLoadTask.result);
    }


    void OnDisable()
    {
        EventManager.instance.Disregister<SessionEventArg>(this, EventName.SessionEvent);
    }

}