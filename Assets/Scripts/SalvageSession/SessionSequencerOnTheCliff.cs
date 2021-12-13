using UnityEngine;

public class SessionSequencerOnTheCliff : ISessionSequencer
{
    //このセッションを主導するEntity
    ArmBotData.Entity entity;
    SectorMap map;
    Vector2 startPosition;
    Vector2? targetCoords = null;

    public SessionSequencerOnTheCliff(SectorMap map, ArmBotData.Entity entity, Vector2 startCords)
    {
        this.entity = entity;
        this.map = map;
        this.startPosition = startCords;
    }

    public SessionSequencerOnTheCliff(SectorMap map, ArmBotData.Entity entity, Vector2 startCords, Vector2 targetCoords)
    {
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
        Vector2 nowCoords = startCoords;

        while (!entity.CheckIfEnd())
        {
            //ここもしかしたら処理重すぎかも
            //Speedはほぼこれが連続的であるとみなせる位の値に設定してください
            var delta = entity.facingDirection.normalized * entity.GetStatus(StatusType.speed) * SessionConfig.instance.speedMultiplier;

            nowCoords += delta;
            var arg = new TravelExArg(entity, nowCoords, delta);
            EventManager.instance.Notice<ExploreArg>(EventName.SystemExploreEvent, arg);

            builder.Append("       Id:");
            builder.AppendLine(entity.id);
            builder.Append("     Type:");
            builder.AppendLine(entity.type.ToString());
            builder.Append("       hp:");
            builder.AppendLine(entity.hp.ToString());
            builder.Append("   Miasma:");
            builder.AppendLine(MapUtility.GetMiasma(nowCoords).ToString());
            builder.Append("MiasmaDam:");
            builder.AppendLine(MapUtility.GetMiasmaDamage(nowCoords).ToString());
            arg.BuildLog(ref builder);

            Utility.LogWriter.Log(builder.ToString(), "TravelLog", true);

            entity.OnInteract(map, nowCoords);
        }

    }
}