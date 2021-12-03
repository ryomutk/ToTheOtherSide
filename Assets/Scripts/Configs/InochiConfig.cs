using System;
using System.Collections.Generic;
using UnityEngine;

public class InochiConfig:SingleScriptableObject<InochiConfig>
{
    [SerializeField] SerializableDictionary<BotType,BotStatusList> maxStatusValues;
    [SerializeField] SerializableDictionary<BotType, int> botCostTable;

    //各種能力値の最大値を確認。なければnullを返す
    public static int? GetMaxValue(BotType botType,StatusType statusType)
    {
        return instance.maxStatusValues[botType].GetValue(statusType);
    }

    public static int GetPurchaseCost(BotType type)
    {
        return instance.botCostTable[type];
    }

    public static BotStatusList GetEvolutionStatusList(ArmBotData.Entity entity)
    {
        var stats = new BotStatusList(entity.statusTypes);
        throw new System.NotImplementedException();
    }
}