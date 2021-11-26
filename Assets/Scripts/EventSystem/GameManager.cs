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

    IEnumerator SystemInitialize()
    {
        yield return 4;

        var task = EventManager.instance.Notice(EventName.SystemEvent,new SystemEventArg(GameState.SystemInitialize));
        yield return new WaitUntil(()=>task.compleated);

        task = EventManager.instance.Notice(EventName.SystemEvent,new SystemEventArg(GameState.ViewInitialize));
        yield return new WaitUntil(()=>task.compleated);
    }

}