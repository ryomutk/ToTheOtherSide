using System;
using System.Collections;
using UnityEngine;

//ViewInitializeで必要なものを見せてくれる人
public class ViewInitializer:MonoBehaviour,IEventListener<SystemEventArg>
{
    void Start()
    {
        EventManager.instance.Register(this,EventName.SystemEvent);
    }

    public ITask OnNotice(SystemEventArg args)
    {
        if(args.state == GameState.ViewInitialize)
        {
            var task = new SmallTask();
            StartCoroutine(InitializeView(task));
            return task;
        }

        return SmallTask.nullTask;
    }

    IEnumerator InitializeView(SmallTask task)
    {
        var arg = new UIEventArg(PanelName.SafeAreaPanel,ShowType.overrap,PanelAction.show); 
        var task1 = EventManager.instance.Notice(EventName.UIEvent,arg);

        yield return new WaitUntil(()=>task1.compleated);


        arg = new UIEventArg(PanelName.IslandPanel,ShowType.overrap,PanelAction.show);
        task1 = EventManager.instance.Notice(EventName.UIEvent,arg);

        yield return new WaitUntil(()=>task1.compleated);

        task.compleated = true;
    }
}