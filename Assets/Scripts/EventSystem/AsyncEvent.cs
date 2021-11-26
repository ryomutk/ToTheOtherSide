using UnityEngine;
using System.Collections.Generic;

public class AsyncEvent : ScriptableObject
{

    public virtual int listeners { get { return registrations.Count; } }
    List<IEventTask> registrations = new List<IEventTask>();
    public void Register(IEventTask fukidashi)
    {
        registrations.Add(fukidashi);
    }

    public bool DisRegister(IEventTask fukidashi)
    {
        return registrations.Remove(fukidashi);
    }


    /// <summary>
    /// Notice event to listeners
    /// </summary>
    /// <returns>true count</returns>
    public virtual SmallTask Notice()
    {
        var tasks = new List<SmallTask>();

        for (int i = 0; i < registrations.Count; i++)
        {
            var task = registrations[i].OnNotice();
            if (!task.ready)
            {
                tasks.Add(task);
            }
        }

        return new TaskBase(
            () =>
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (!tasks[i].ready)
                    {
                        return false;
                    }
                };

                return true;
            }
        );
    }

    void OnDisable()
    {
        registrations.Clear();
    }
}

public class AsyncEvent<T> : AsyncEvent, ISalvageData
{
    public override int listeners { get { return base.listeners + registrations.Count; } }
    List<IEventTask<T>> registrations = new List<IEventTask<T>>();
    public void Register(IEventTask<T> fukidashi)
    {
        registrations.Add(fukidashi);
    }

    public bool DisRegister(IEventTask<T> fukidashi)
    {
        return registrations.Remove(fukidashi);
    }

    /// <summary>
    /// りすなーさんたちにargをnotice!
    /// </summary>
    /// <param name="arg">あーぎゅめんと</param>
    /// <returns>true count,なんかしたらtrueを返す予定</returns>
    public SmallTask Notice(T arg)
    {
        var baseTask = base.Notice();

        var tasks = new List<SmallTask>();

        for (int i = 0; i < registrations.Count; i++)
        {
            var task = registrations[i].OnNotice(arg);
            if (!task.ready)
            {
                tasks.Add(task);
            }
        }

        return new TaskBase(
            () =>
            {
                if(!baseTask.ready)
                {
                    return false;
                }

                for (int i = 0; i < tasks.Count; i++)
                {
                    if (!tasks[i].ready)
                    {
                        return false;
                    }
                };

                return true;
            }
        );
    }

    //事故防止のために使えなくしちゃう
    public override SmallTask Notice()
    {
        throw new System.Exception("This is Generic Event. Use Notice<T> otherwise");
    }

    void OnDisable()
    {
        Debug.Log("Cleared");
        registrations.Clear();
    }

}