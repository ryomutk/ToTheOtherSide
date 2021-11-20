using UnityEngine;

public abstract class ToolData:ItemData
{ 
    public abstract void OnUse(ArmBotData.Entity player);
}