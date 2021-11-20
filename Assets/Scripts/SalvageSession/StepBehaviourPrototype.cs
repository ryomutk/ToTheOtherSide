using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class StepBehaviourPrototype : SectorStep.StepBehaviourBase
{
    //これを持っていないと入れないもの
    [SerializeField] List<ItemID> accRequirement = new List<ItemID>();
    [SerializeField] List<AccModData> accMods = new List<AccModData>();
    [SerializeField] List<searchPModData> searchPMods = new List<searchPModData>();

    //これを持っているとAccが変わるものとそのボーナス値
    [Serializable]
    class AccModData
    {
        [SerializeField] public ItemID item;
        [SerializeField] public int bonus;
    }

    [Serializable]
    class searchPModData
    {
        [SerializeField] public ItemID item;
        [SerializeField] public int bonus;
    }

    [SerializeField]

    public override int GetAccessability(ArmBotData.Entity bot, SectorStep step)
    {
        var acc = base.GetAccessability(bot, step);

        foreach (var req in accRequirement)
        {
            if (!bot.HasEquip(req))
            {
                acc = -1;
            }
        }

        if (acc > 0)
        {
            foreach (var mod in accMods)
            {
                if(bot.HasEquip(mod.item))
                {
                    acc += mod.bonus;
                }
            }

            if(acc<0)
            {
                acc = 1;
            }
        }

        return acc;
    }

    public override void OnEnter(ArmBotData.Entity bot, SectorStep step)
    {
        base.OnEnter(bot, step);
        foreach (var mod in searchPMods)
        {
            if (bot.HasEquip(mod.item))
            {
                searchPbonus += mod.bonus;
            }
        }
    }

    public override void OnExit(ArmBotData.Entity bot, SectorStep step)
    {
        base.OnExit(bot, step);
        searchPbonus = 0;
    }
}