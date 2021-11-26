using UnityEngine;
using System.Collections.Generic;

public class AsyncEvent:ScriptableObject
{

    public virtual int listeners{get{return registrations.Count;}}
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
            if(!task.ready)
            {
                tasks.Add(task);
            }
        }

        return new SmallTask();
    }

    void OnDisable()
    {
        registrations.Clear();
    }


}