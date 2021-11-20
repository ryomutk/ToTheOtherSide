using UnityEngine;
using System.Collections.Generic;
using System;

//EquipmentとBotの協会平面絵画
//特殊なことがしたきゃBotでやれ
[Serializable]
public class EquipmentBehaviour
{

    [SerializeField] List<BotModifier> mods;
    public void OnEquip(ArmBotData bot)
    {
        foreach(var mod in mods)
        {
            mod.Apply(bot);
        }
    }

    public void OnRemove(ArmBotData bot)
    {
        foreach(var mod in mods)
        {
            mod.Apply(bot);
        }
    }
}