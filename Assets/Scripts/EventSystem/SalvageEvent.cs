using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class SalvageEvent : ScriptableObject, ISalvageData,IEvent
{
    public virtual int listeners{get{return registrations.Count;}}
    List<IEventListener> registrations = new List<IEventListener>();
    public void Register(IEventListener fukidashi)
    {
        registrations.Add(fukidashi);
    }

    public bool DisRegister(IEventListener fukidashi)
    {
        return registrations.Remove(fukidashi);
    }


    /// <summary>
    /// Notice event to listeners
    /// </summary>
    /// <returns>true count</returns>
    public virtual ITask Notice()
    {
        var count = 0;
        for (int i = 0; i < registrations.Count; i++)
        {
            if(registrations[i].OnNotice())
            {
                count++;
            }
        }

        return SmallTask.nullTask;
    }

    void OnDisable()
    {
        registrations.Clear();
    }

}

public class SalvageEvent<T> : SalvageEvent, ISalvageData,IEvent<T>
where T:SalvageEventArg
{
    public override int listeners{get{return base.listeners + registrations.Count;}}
    [ShowInInspector,ReadOnly]List<IEventListener<T>> registrations = new List<IEventListener<T>>();
    public void Register(IEventListener<T> fukidashi)
    {
        registrations.Add(fukidashi);
    }

    public bool DisRegister(IEventListener<T> fukidashi)
    {
        return registrations.Remove(fukidashi);
    }

    /// <summary>
    /// りすなーさんたちにargをnotice!
    /// </summary>
    /// <param name="arg">あーぎゅめんと</param>
    /// <returns>true count,なんかしたらtrueを返す予定</returns>
    public ITask Notice(T arg)
    {
        base.Notice();
        var count = 0;
        for (int i = 0; i < registrations.Count; i++)
        {
            if(registrations[i].OnNotice(arg))
            {
                count++;
            }
        }

        return SmallTask.nullTask;
    }

    //事故防止のために使えなくしちゃう
    public override ITask Notice()
    {
        throw new System.Exception("This is Generic Event. Use Notice<T> otherwise");
    }

    void OnDisable()
    {
        Debug.Log("Cleared");
        registrations.Clear();
    }

}