using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

//Interactに"加えて"おこるハプニング的な奴
[Serializable]
public class InterctBehaviour
{
    [ShowInInspector]
    List<InteractEvent> events{get{return _events;} set{_events = value;}}
    [SerializeField,HideInInspector] List<InteractEvent> _events;

    public void OnInteract(ArmBotData.Entity botEntity, SectorStep step)
    {

        for (int i = 0; i < events.Count; i++)
        {
            var dice = UnityEngine.Random.Range(0f, 1);
            if(dice < events[i].rate)
            {
                events[i].Apply(botEntity,step);
            }
        }
    }
}