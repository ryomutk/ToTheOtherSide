using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inochi/MOTHER")]
public class MOTHERData : ArmBotData
{
    [SerializeField] int maxBarrier;
    protected override Entity CreateInstance()
    {
        return new MotherEntity(this);
    }

    public override BotType type{get{return BotType.MOTHER;}}

    protected override BotStatusList status{get{return _status;}}
    [SerializeField] BotStatusList _status = new BotStatusList(StatusType.hp,StatusType.speed,StatusType.tomoshibi,StatusType.stock,StatusType.fuelRate,StatusType.sheldLevel,StatusType.LV);

    public class MotherEntity:Entity
    {
        int maxBarrier;
        public int GetBarrer()
        {
            var tomoshibi = nowStatus.GetValue(StatusType.tomoshibi).Value;
            var maxTomoshibi = InochiConfig.GetMaxValue(type,StatusType.tomoshibi).Value;
            var norm = (tomoshibi/maxTomoshibi);             
            norm *= norm;

            return (int)maxBarrier*norm;
        }

        public MotherEntity(ArmBotData data):base(data,BotType.MOTHER)
        {
            this.maxBarrier = ((MOTHERData)data).maxBarrier;
        }

        public override bool CheckIfEnd()
        {
            return hp >= 0;
        }

        public override bool OnUpdate(SectorMap map, Vector2 coordinate)
        {
            throw new System.NotImplementedException();
        }

    }
}