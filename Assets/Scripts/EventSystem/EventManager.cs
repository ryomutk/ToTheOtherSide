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
        StartCoroutine(LoadEvent(task,eventName));

    }

    public SmallTask Register<T>(IEventListener<T> listener)
    {

    }

    IEnumerator LoadEvent(SmallTask task,EventName name,IEventListener listener)
    {
        var label = eventLabelTable[name];
        
        var loadtask = DataManager.LoadDataAsync(label);
        yield return new WaitUntil(()=>loadtask.ready);


    }
}