using UnityEngine;


[CreateAssetMenu(menuName = "Inochi/MOTHER")]
public class MOTHERData : ArmBotData
{
    protected override Entity CreateInstance()
    {
        throw new System.NotImplementedException();
    }

    protected override BotStatusList status{get{return _status;}}
    [SerializeField] BotStatusList _status = new BotStatusList(StatusType.hp,StatusType.speed,StatusType.tomoshibi,StatusType.stock,StatusType.fuelRate,StatusType.sheldLevel);

    public class MotherEntity:Entity
    {
        public MotherEntity(ArmBotData data):base(data,BotType.MOTHER)
        {

        }

        public override bool CheckIfEnd()
        {
            return hp >= 0;
        }

        public override BotStatusList Evolution(int resourceNum, bool distructive = false)
        {
            throw new System.NotImplementedException();
        }

        public override bool OnInteract(SectorMap map, Vector2 coordinate)
        {
            throw new System.NotImplementedException();
        }
    }
}