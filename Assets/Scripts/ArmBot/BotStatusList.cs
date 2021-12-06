using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

//キャラクターのステータス値を保持するやつ。
//インスタンスでも使うのでScriptableにしない
[Serializable]
public class BotStatusList : ISalvageData
{
    public BotStatusList(params StatusType[] states)
    {
        this.types = states;
        foreach (var state in states)
        {
            values[state] = 0;
        }
    }

    [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
    SerializableDictionary<StatusType, int> values = new SerializableDictionary<StatusType, int>();

    public StatusType[] types{get;}
    public void SetValue(StatusType type, int value)
    {
        values[type] = value;
    }

    public int? GetValue(StatusType type)
    {
        return values[type];
    }

    public BotStatusList CopySelf()
    {
        var copy = new BotStatusList(this.types);

        foreach (var item in values)
        {
            copy.SetValue(item.key,item.value);
        }
        
        return copy;
    }
}