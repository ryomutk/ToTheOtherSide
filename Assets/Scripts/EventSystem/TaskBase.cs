using System;

public class TaskBase : SmallTask
{
    public override bool ready
    {
        get {return endChecker();}
        set => base.ready = value;
    }

    Func<bool> endChecker;

    public TaskBase(Func<bool> endChecker)
    {
        this.endChecker = endChecker;
    }
}