using UnityEngine;
using System;

[Serializable]
public class InteractEvent
{
    public float rate{get{return _rate;}} 
    [SerializeField,Range(0,1)] float _rate;
    public virtual void Apply(ArmBotData.Entity entity,SectorStep step){}
}

[Serializable]
public class GetItemEvent:InteractEvent
{
    public ItemID iD;

    public override void Apply(ArmBotData.Entity entity, SectorStep step)
    {
        entity.GiveItem(iD);
    }
}
