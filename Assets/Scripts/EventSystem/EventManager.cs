using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
public enum EventName
{
    none,
    ExploreEvent
}

public class EventManager:Singleton<EventManager>
{
    [SerializeField] SerializableDictionary<EventName,AssetLabelReference> eventLabelTable; 
    
}