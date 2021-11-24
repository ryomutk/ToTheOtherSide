/*
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

public class ExploreEventBuilder : IEventListener<ExploreArg>
{
    public List<ExploreArg> allEvents { get { return eventsPublished; } }
    [ShowInInspector]
    public Dictionary<int, List<ExploreArg>> timeline { get { return eventTimeLine; } }
    List<ExploreArg> eventsPublished = new List<ExploreArg>();
    Dictionary<int, List<ExploreArg>> eventTimeLine = new Dictionary<int, List<ExploreArg>>();
    int stepCount;


    public ExploreEventBuilder()
    {
        eventTimeLine[0] = new List<ExploreArg>(); 
    }

    public bool OnNotice(ExploreArg arg)
    {
        if (arg.type == ExploreObjType.Interact)
        {
            stepCount++;
            eventTimeLine[stepCount] = new List<ExploreArg>();
        }
        
        eventsPublished.Add(arg);
        eventTimeLine[stepCount].Add(arg);

        return true;
    }
}
*/