using UnityEngine;
using System;
using System.Collections.Generic;

//キャラクターのステータス値を保持するやつ。
//インスタンスでも使うのでScriptableにしない
[Serializable]
public class BotStatusList:ISalvageData
{
    public BotStatusList()
    {
        var status = Enum.GetValues(typeof(StatusType)) as StatusType[];

        foreach(var state in status)
        {
            var data = new LabeledData<StatusType,int>();
            data.type = state;
            values.Add(data);
        }
    }
    [SerializeField]List<LabeledData<StatusType,int>> values = new List<LabeledData<StatusType, int>>();

    public void SetValue(StatusType type,int value)
    {
        for(int i = 0;i < values.Count;i++)
        {
            if(values[i].type == type)
            {
                values[i].value = value;
                return;
            }
        }

        throw new KeyNotFoundException();
    }

    public int GetValue(StatusType type)
    {
        for(int i = 0;i < values.Count;i++)
        {
            if(values[i].type == type)
            {
                return values[i].value;
            }
        }

        throw new KeyNotFoundException();
    }

    public BotStatusList CopySelf()
    {
        var copy = new BotStatusList();
        
        foreach(var value in values)
        {
            copy.SetValue(value.type,value.value);
        }

        return copy;
    }
}