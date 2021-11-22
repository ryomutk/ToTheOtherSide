using System.Text;
using System;
using UnityEngine;

[Serializable]
public abstract class ExploreArg:ISEventArg
{
    public abstract ExploreObjType type { get; }
    public abstract bool Equals(ExploreArg arg);

#if UNITY_EDITOR
    public abstract void BuildLog(ref StringBuilder builder);
#endif
}

public class SerializableExArg : ExploreArg
{
    public override ExploreObjType type => throw new NotImplementedException();
    public override void BuildLog(ref StringBuilder builder)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(ExploreArg arg)
    {
        throw new NotImplementedException();
    }
}



//DepthDeltaも持ってるのあんまよくないかも…
//とったアクションだけを記録したいので…
[Serializable]
public class StepActionArg : SerializableExArg
{
    public override ExploreObjType type { get { return ExploreObjType.Interact; } }
    public StepActionArg(StepActionType type, float delta, int id)
    {
        this.depthDelta = delta;
        this.stepId = id;
        this.actionType = type;
    }

    public override bool Equals(ExploreArg obj)
    {
        if (obj is StepActionArg arg)
        {
            return arg.depthDelta == this.depthDelta &&
            this.stepId == arg.stepId &&
            this.actionType == arg.actionType;
        }
        return false;
    }

    public StepActionType actionType;
    public float depthDelta;
    public int stepId;


#if UNITY_EDITOR
    public override void BuildLog(ref StringBuilder builder)
    {
        builder.AppendLine();
        builder.AppendLine("--STEP ACTION--");
        builder.Append("actionType:");
        builder.AppendLine(actionType.ToString());
        builder.Append("depthDelta:");
        builder.AppendLine(depthDelta.ToString());
        builder.Append("    stepId:");
        builder.AppendLine(stepId.ToString());
        builder.AppendLine();
    }
#endif
}



[Serializable]
public class StepExArg : SerializableExArg
{
    public override ExploreObjType type { get { return ExploreObjType.Step; } }

    public StepExArg(int iD, StepState state)
    {
        this.step = iD;
        this.state = state;
    }

    public override bool Equals(ExploreArg arg)
    {
        if (arg is StepExArg arg1)
        {
            return this.step == arg1.step && this.state == arg1.state;
        }
        return false;
    }
    public int step;
    public StepState state;

#if UNITY_EDITOR
    public override void BuildLog(ref StringBuilder builder)
    {
        builder.AppendLine();
        builder.AppendLine("--STEPEXPLORE--");
        builder.Append("StepId:");
        builder.AppendLine(step.ToString());
        builder.Append(" State:");
        builder.AppendLine(state.ToString());
        builder.AppendLine();
    }
#endif
}

[Serializable]
public class ItemExArg : SerializableExArg
{
    public override ExploreObjType type { get { return ExploreObjType.Item; } }
    public ItemExArg(ItemID iD, ItemActionType type)
    {
        this.itemID = iD;
        this.actionType = type;
    }
    public ItemID itemID;
    public ItemActionType actionType;

    public override bool Equals(ExploreArg obj)
    {
        if (obj is ItemExArg arg)
        {
            return arg.itemID == this.itemID &&
            this.actionType == arg.actionType;
        }
        return false;
    }

#if UNITY_EDITOR
    public override void BuildLog(ref StringBuilder builder)
    {
        builder.AppendLine();
        builder.AppendLine("--ITEM--");
        builder.Append("     ItemID:");
        builder.AppendLine(itemID.ToString());
        builder.Append(" actionType:");
        builder.AppendLine(actionType.ToString());
        builder.AppendLine();
    }
#endif
}

[Serializable]
public class EventExArg : SerializableExArg
{
    public override ExploreObjType type { get { return ExploreObjType.Event; } }
    public EventExArg()
    {
        throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    public override void BuildLog(ref StringBuilder builder)
    {
        throw new System.NotImplementedException();
    }
#endif

}