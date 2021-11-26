using System.Text;
using System;
using UnityEngine;

[Serializable]
public abstract class ExploreArg : SalvageEventArg
{
    public ArmBotData.Entity from { get; }
    public abstract ExploreObjType type { get; }
    public abstract bool Equals(ExploreArg arg);
    public ExploreArg(ArmBotData.Entity from)
    {
        this.from = from;
    }

#if UNITY_EDITOR
    public abstract void BuildLog(ref StringBuilder builder);
#endif
}

public class SerializableExArg : ExploreArg
{
    public SerializableExArg(ArmBotData.Entity from) : base(from) { }

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


//いちじょーほー
public class TravelExArg : SerializableExArg
{
    public Vector2 coordinate { get; }
    public Vector2 traveledVec { get; }

    public TravelExArg(ArmBotData.Entity from, Vector2 coordinate, Vector2 traveledVec) : base(from)
    {
        this.coordinate = coordinate;
        this.traveledVec = traveledVec;
    }

#if UNITY_EDITOR
    public override void BuildLog(ref StringBuilder builder)
    {
        builder.AppendLine();
        builder.AppendLine("--TRAVELED ARG--");
        builder.Append("coordinate:");
        builder.AppendLine(coordinate.ToString());
        builder.Append("  traveled:");
        builder.AppendLine(traveledVec.ToString());
        builder.AppendLine();
    }
#endif
}

//DepthDeltaも持ってるのあんまよくないかも…
//とったアクションだけを記録したいので…
[Serializable]
public class StepActionArg : SerializableExArg
{
    public override ExploreObjType type { get { return ExploreObjType.Interact; } }
    public StepActionArg(ArmBotData.Entity from, StepActionType type, int id) : base(from)
    {
        this.stepId = id;
        this.actionType = type;
    }

    public override bool Equals(ExploreArg obj)
    {
        if (obj is StepActionArg arg)
        {
            return
            this.stepId == arg.stepId &&
            this.actionType == arg.actionType;
        }
        return false;
    }

    public StepActionType actionType;
    public int stepId;


#if UNITY_EDITOR
    public override void BuildLog(ref StringBuilder builder)
    {
        builder.AppendLine();
        builder.AppendLine("--STEP ACTION--");
        builder.Append("actionType:");
        builder.AppendLine(actionType.ToString());
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

    public StepExArg(ArmBotData.Entity from, int iD, StepState state) : base(from)
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
    public ItemExArg(ArmBotData.Entity from, ItemID iD, ItemActionType type) : base(from)
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
    public EventExArg(ArmBotData.Entity from) : base(from)
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