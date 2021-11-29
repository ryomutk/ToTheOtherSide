using UnityEngine;
using System.Collections.Generic;
public class SearcherData : ArmBotData
{
    BotStatusList statusList = new BotStatusList(StatusType.hp, StatusType.speed, StatusType.search);
    protected override BotStatusList status { get { return statusList; } }
    protected override Entity CreateInstance(Vector2 faceDirection)
    {
        return new SearcherEntity(this,faceDirection);
    }

    public class SearcherEntity : Entity
    {
        public SearcherEntity(ArmBotData data, Vector2 direction) : base(data, direction)
        { }

        List<SectorStep> _foundSteps = new List<SectorStep>();
        public List<SectorStep> foundSteps { get { return _foundSteps; } private set { _foundSteps = value; } }

        public override bool OnInteract(SectorMap map, Vector2 coordinate)
        {
            //発見報告をするンゴねぇ
            //目視で角煮！
            var result = map.TryFindRange(coordinate, GetStatus(StatusType.search), ref _foundSteps);

            if (result > 0)
            {
                for (int i = 0; i < result; i++)
                {
                    var arg = new StepActionArg(this, StepActionType.confirm, foundSteps[foundSteps.Count - i].id);
                    EventManager.instance.Notice(EventName.SystemExploreEvent,arg);
                }
            }

            return true;
        }

        public override bool CheckIfEnd()
        {
            //"セッション"終了
            if (hp < 0)
            {
                return true;
            }

            return false;
        }
    }

}