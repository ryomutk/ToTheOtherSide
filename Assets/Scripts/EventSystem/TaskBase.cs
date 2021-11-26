using System;
using UnityEngine;

public class TaskBase : SmallTask
{
    public override bool ready
    {
        get {return endChecker();}
        set {Debug.LogWarning("This is not small task");}
    }

    Func<bool> endChecker;

    public TaskBase(Func<bool> endChecker)
    {
        this.endChecker = endChecker;
    }
}