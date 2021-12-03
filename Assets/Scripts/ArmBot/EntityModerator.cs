using UnityEngine;
using System;

[Serializable]
//EntityがAしたときにBするためのモディふぃけーたー
public class EntityMod
{
    public string discription { get; }
    public StepActionType exTiming { get; }
    Action<ArmBotData.Entity> action;

    public EntityMod(StepActionType onWhichAction,Action<ArmBotData.Entity> action,string discription = null)
    {
        this.exTiming = onWhichAction;
        this.discription = discription;
        this.action = action;
    }

    public void Execute(ArmBotData.Entity entity)
    {
        action(entity);
    }
}