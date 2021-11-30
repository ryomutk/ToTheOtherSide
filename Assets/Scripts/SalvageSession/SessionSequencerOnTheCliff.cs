using UnityEngine;

public class SessionSequencerOnTheCliff : ISessionSequencer
{
    //このセッションを主導するEntity
    ArmBotData.Entity entity;
    SectorMap map;
    Vector2 startPosition;

    public SessionSequencerOnTheCliff(SectorMap map, ArmBotData.Entity entity, Vector2 startCords)
    {
        this.entity = entity;
        this.map = map;
        this.startPosition = startCords;
    }

    public void BuildSession()
    {
        StepRoutine(startPosition);
    }


    void StepRoutine(Vector2 startCoords)
    {
        Vector2 nowCoords = startCoords;

        while (entity.CheckIfEnd())
        {
            //ここもしかしたら処理重すぎかも
            //Speedはほぼこれが連続的であるとみなせる位の値に設定してください
            var delta = entity.facingDirection * entity.GetStatus(StatusType.speed) * SessionConfig.instance.speedMultiplier;

            nowCoords += delta;
            EventManager.instance.Notice(EventName.SystemExploreEvent, new TravelExArg(entity, nowCoords, delta));
        }

    }
}