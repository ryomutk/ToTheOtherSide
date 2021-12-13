using UnityEngine;
public class BotEventArg:SalvageEventArg
{
    public ArmBotData.Entity from{get;}
    public BotActionType type{get;}
    public StatusType modifiedStat{get;}
    public int delta{get;}

    public BotEventArg(ArmBotData.Entity sender,BotActionType type,StatusType modifiedStat = StatusType.none,int delta = 0)
    {
        this.from = sender;
        this.type = type;
        this.modifiedStat = modifiedStat;
        this.delta = delta;
    }
}