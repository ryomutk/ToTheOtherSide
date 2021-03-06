using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inochi/Searcher")]
public class SearcherData : ArmBotData
{
    public override BotType type{get{return BotType.searcher;}}
    [SerializeField]BotStatusList statusList = new BotStatusList(StatusType.hp, StatusType.speed, StatusType.search,StatusType.LV);
    protected override BotStatusList status { get { return statusList; } }
    protected override Entity CreateInstance()
    {
        return new SearcherEntity(this);
    }

    public class SearcherEntity : Entity
    {
        public SearcherEntity(ArmBotData data) : base(data,BotType.searcher)
        { }

        List<int> _foundSteps = new List<int>();
        public List<int> foundSteps { get { return _foundSteps; } private set { _foundSteps = value; } }

        public override bool OnInteract(SectorMap map, Vector2 coordinate)
        {
            //発見報告をするンゴねぇ
            //目視で角煮！
            var result = map.TryFindRange(coordinate, GetStatus(StatusType.search), ref _foundSteps);

            if (result > 0)
            {
                for (int i = 1; i <= result; i++)
                {
                    //見えた島を通知
                    var arg = new StepActionArg(this, StepActionType.confirm, foundSteps[foundSteps.Count - i]);
                    EventManager.instance.Notice<ExploreArg>(EventName.SystemExploreEvent,arg);
                }
            }

            hp -= (int)MapUtility.GetMiasmaDamage(coordinate);

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