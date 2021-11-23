using UnityEngine;

public class SearcherData:ArmBotData
{
    BotStatusList statusList = new BotStatusList(StatusType.hp,StatusType.speed,StatusType.search);
    protected override BotStatusList status{get{return statusList;}}

    protected override bool OnInteract(Entity entity,SectorStep step)
    {   
        //発見報告をするンゴねぇ
        //目視で角煮！
        var arg = new StepActionArg(entity,StepActionType.confirm,step.id);
        exploreEvent.Notice(arg);

        return true;
    }

    protected override bool CheckIfEnd(Entity entity)
    {
        //"セッション"終了
        if(entity.hp < 0)
        {
            return true;
        }

        return false;
    }

}