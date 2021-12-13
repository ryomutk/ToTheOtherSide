using UnityEngine;

public class SessionSequencerOnTheCliff : ISessionSequencer
{
    //このセッションを主導するEntity
    ArmBotData.Entity entity;
    SectorMap map;
    Vector2 startPosition;
    Vector2? targetCoords = null;
    SessionData selfData;

    public SessionSequencerOnTheCliff(SectorMap map, ArmBotData.Entity entity, Vector2 startCords,SessionData selfData)
    {
        this.selfData = selfData;
        this.entity = entity;
        this.map = map;
        this.startPosition = startCords;
    }

    public SessionSequencerOnTheCliff(SectorMap map, ArmBotData.Entity entity, Vector2 startCords, Vector2 targetCoords,SessionData selfData)
    {
        this.selfData = selfData;
        this.entity = entity;
        this.map = map;
        this.startPosition = startCords;
        this.targetCoords = targetCoords;
    }

    public void BuildSession()
    {
        StepRoutine(startPosition);
    }

    System.Text.StringBuilder builder = new System.Text.StringBuilder();
    void StepRoutine(Vector2 startCoords)
    {
        while (!entity.CheckIfEnd())
        {
            //ここもしかしたら処理重すぎかも
            //Speedはほぼこれが連続的であるとみなせる位の値に設定してください

            builder.Append("       Id:");
            builder.AppendLine(entity.id);
            builder.Append("     Type:");
            builder.AppendLine(entity.type.ToString());
            builder.Append("       hp:");
            builder.AppendLine(entity.hp.ToString());
            builder.Append("   Miasma:");
            builder.AppendLine(MapUtility.GetMiasma(selfData.nowCoordinate).ToString());
            builder.Append("MiasmaDam:");
            builder.AppendLine(MapUtility.GetMiasmaDamage(selfData.nowCoordinate).ToString());

            Utility.LogWriter.Log(builder.ToString(), "TravelLog", true);

            entity.OnInteract(map, selfData.nowCoordinate);
        }

    }
}