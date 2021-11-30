using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inochi/Searcher")]
public class SearcherData : ArmBotData
{
    [SerializeField]BotStatusList statusList = new BotStatusList(StatusType.hp, StatusType.speed, StatusType.search);
    protected override BotStatusList status { get { return statusList; } }
    protected override Entity CreateInstance()
    {
        return new SearcherEntity(this);
    }

    public class SearcherEntity : Entity
    {
        public SearcherEntity(ArmBotData data) : base(data,BotType.searcher)
        { }

        List<Island> _foundSteps = new List<Island>();
        public List<Island> foundSteps { get { return _foundSteps; } private set { _foundSteps = value; } }

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

        public override BotStatusList Evolution(int resourceNum,bool distructive)
        {
            throw new System.NotImplementedException();
        }
    }

}