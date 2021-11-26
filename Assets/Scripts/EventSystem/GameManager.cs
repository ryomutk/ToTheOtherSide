using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Singleton<GameManager>
{

    void Start()
    {
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
        
        var task = EventManager.instance.Notice(EventName.SystemEvent,new SystemEventArg(GameState.SystemInitialize));
        yield return new WaitUntil(()=>task.compleated);

       task = EventManager.instance.Notice(EventName.SystemEvent,new SystemEventArg(GameState.ViewInitialize));
       yield return new WaitUntil(()=>task.compleated);
    }

}