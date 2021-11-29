using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public abstract class TouchPanel : MonoBehaviour, IEventListener<ScreenTouchArg>, IEventListener<SystemEventArg>
{
    [SerializeField] RectTransform targetTransform;
    [SerializeField] GameState[] targetStates;

    protected virtual void Start()
    {
        EventManager.instance.Register<SystemEventArg>(this, EventName.SystemEvent);
    }

    public abstract ITask OnNotice(ScreenTouchArg arg);
    public ITask OnNotice(SystemEventArg arg)
    {
        if (targetStates.Contains(arg.state))
        {
            return EventManager.instance.Register<ScreenTouchArg>(this, EventName.ScreenTouchEvent);
        }
        else
        {
            EventManager.instance.Disregister<ScreenTouchArg>(this, EventName.ScreenTouchEvent);
            return SmallTask.nullTask;
        }
    }

    protected virtual void OnDisable()
    {
        //開いたままDisableすると残っちゃうので、それを防ぐため
        EventManager.instance.Disregister<ScreenTouchArg>(this,EventName.ScreenTouchEvent);
        
        EventManager.instance.Disregister<SystemEventArg>(this, EventName.SystemEvent);
    }

}