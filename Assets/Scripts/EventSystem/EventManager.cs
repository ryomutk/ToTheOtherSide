using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Collections;
public enum EventName
{
    none,
    SystemExploreEvent,
    RealtimeExploreEvent,
    SessionEvent
}

public class EventManager : Singleton<EventManager>
{
    [SerializeField] SerializableDictionary<EventName, AssetLabelReference> eventLabelTable = new SerializableDictionary<EventName, AssetLabelReference>();
    Dictionary<EventName, SalvageEvent> eventTable = new Dictionary<EventName, SalvageEvent>();

    public int Notice(EventName name)
    {
        if(eventTable.TryGetValue(name,out var eve))
        {
            return eve.Notice();
        }

        #if DEBUG
        Debug.LogWarning("no one is listening"+name);
        #endif

        return 0;
    }

    public int Notice<T>(EventName name,T arg)
    where T:ISalvageEventArg
    {
        if(eventTable.TryGetValue(name,out var eve))
        {
            var teve = eve as SalvageEvent<T>;
            return teve.Notice(arg);
        }

        #if DEBUG
        Debug.LogWarning("no one is listening"+name);
        #endif

        return 0;
    }

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
    where T : ISalvageEventArg
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

            throw new System.Exception(ev + " is not salvageEvent of type " + typeof(T));
        }
    }

    public bool Disregister<T>(IEventListener<T> listener, EventName name)
    where T : ISalvageEventArg
    {
        var eve = eventTable[name] as SalvageEvent<T>;
        if (eve != null)
        {
            var result = eve.DisRegister(listener);
            if (eve.listeners == 0)
            {
                ReleaseEvent(name);
            }
        }

        #if DEBUG
        Debug.LogWarning("event is null");
        #endif

        return false;
    }

    public bool Disregister(IEventListener listener, EventName name)
    {
        var eve = eventTable[name];
        if (eve != null)
        {
            var result = eve.DisRegister(listener);
            if (eve.listeners == 0)
            {
                ReleaseEvent(name);
            }
        }

        #if DEBUG
        Debug.LogWarning("event is null");
        #endif
        
        return false;
    }

    void ReleaseEvent(EventName name)
    {
        var eve = eventTable[name];
        DataManager.ReleaseData(eve);
        eventTable.Remove(name);
    }

    IEnumerator LoadEvent(SmallTask task, EventName name)
    {
        var label = eventLabelTable[name];

        var loadtask = DataManager.LoadDataAsync(label);
        yield return new WaitUntil(() => loadtask.ready);
    }
}