using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Collections;
public enum EventName
{
    none,
    ExploreEvent
}

public class EventManager : Singleton<EventManager>
{
    [SerializeField] SerializableDictionary<EventName, AssetLabelReference> eventLabelTable = new SerializableDictionary<EventName, AssetLabelReference>();
    Dictionary<EventName, SalvageEvent> eventTable = new Dictionary<EventName, SalvageEvent>();

    public SmallTask Register(IEventListener listener, EventName eventName)
    {
        if (eventTable.TryGetValue(eventName, out SalvageEvent ev))
        {
            ev.Register(listener);
            return SmallTask.nullTask;
        }

        var task = new SmallTask();
        task.onReady += () =>
        {
            var eve = eventTable[eventName];
            eve.Register(listener);
        };

        StartCoroutine(LoadEvent(task, eventName));

        return task;
    }

    public SmallTask Register<T>(IEventListener<T> listener, EventName eventName)
    where T : ISEventArg
    {
        try
        {
            if (eventTable.TryGetValue(eventName, out SalvageEvent ev))
            {
                var tev = ev as SalvageEvent<T>;
                tev.Register(listener);
                return SmallTask.nullTask;
            }

            var task = new SmallTask();
            task.onReady += () =>
            {
                var eve = eventTable[eventName] as SalvageEvent<T>;
                eve.Register(listener);
            };

            StartCoroutine(LoadEvent(task, eventName));

            return task;
        }
        catch (System.NullReferenceException)
        {
            var ev = eventTable[eventName];

            throw new System.Exception(ev +" is not salvageEvent of type " + typeof(T));
        }
    }

    IEnumerator LoadEvent(SmallTask task, EventName name)
    {
        var label = eventLabelTable[name];

        var loadtask = DataManager.LoadDataAsync(label);
        yield return new WaitUntil(() => loadtask.ready);


    }
}