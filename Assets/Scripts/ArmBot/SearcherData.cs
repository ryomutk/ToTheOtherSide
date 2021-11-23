using UnityEngine;

public class SearcherData:ArmBotData
{
    BotStatusList statusList = new BotStatusList(StatusType.hp,StatusType.speed,StatusType.search);
    protected override BotStatusList status{get{return statusList;}}

    protected override bool InteractAction(SectorStep step)
    {
        new 
    }

    protected override bool EndStatus(Entity entity)
    {
        throw new System.NotImplementedException();
    }
}