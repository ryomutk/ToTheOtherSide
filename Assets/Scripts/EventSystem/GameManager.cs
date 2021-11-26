using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    InteraptorQueue interaptorQueue;

    void Start()
    {
        interaptorQueue = new InteraptorQueue(this);
        StartCoroutine(SystemInitialize());
    }

    /*
    public void RegistInteraptor(SmallTask task)
    {
        interaptorQueue.RegisterQueue(task);
    }
    */

    IEnumerator SystemInitialize()
    {
        yield return 4;

        var sysEvents = Enum.GetValues(typeof(GameState)) as GameState[];

        /*
        //あぁ～これはいい実装では？
        foreach (var state in sysEvents)
        {
            var events = OnSystemEvent.GetInvocationList();

            for(int i =0;i < events.Length;i++)
            {
                var ev = events[i] as Func<GameState,SmallTask>;
                var task = ev(state);
                interaptorQueue.RegisterQueue(task);
            }
            yield return StartCoroutine(HandleInteraptor());
        }
        */
        
        EventManager.instance.Notice(EventName.SystemEvent,new SystemEventArg(GameState.SystemInitialize));
        
        EventManager.instance.Notice(EventName.SystemEvent,new SystemEventArg(GameState.ViewInitialize));
    }

    IEnumerator HandleInteraptor()
    {
        interaptorQueue.SolveQueue();
        while (interaptorQueue.working)
        {
            yield return null;
        }
    }


}