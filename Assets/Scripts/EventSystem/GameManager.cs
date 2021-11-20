using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    //RegisterQueueが嫌いになっちゃったので、Taskを返してもらうことにしました
    //お前がSystemEvent持ってる状況も嫌いになってきたので、近々消える可能性があります
    public event Func<SystemState,SmallTask> OnSystemEvent;
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

        var sysEvents = Enum.GetValues(typeof(SystemState)) as SystemState[];

        //あぁ～これはいい実装では？
        foreach (var state in sysEvents)
        {
            var events = OnSystemEvent.GetInvocationList();

            for(int i =0;i < events.Length;i++)
            {
                var ev = events[i] as Func<SystemState,SmallTask>;
                var task = ev(state);
                interaptorQueue.RegisterQueue(task);
            }
            yield return StartCoroutine(HandleInteraptor());
        }
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