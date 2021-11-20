using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[Serializable]
public class SerializableExploreAndLogDataSet : SalvagetimeExArgAndSDataset<LogData>
{
    public override LogData Trigger(ExploreArg arg)
    {
        return null;
    }

    public override SalvagetimeExArgAndSDataset<LogData> Clone()
    {
        throw new NotImplementedException();
    }
}

//特定のExArgが来たとき、そのState
[Serializable]
public class StepExploreUniqueLogData : SerializableExploreAndLogDataSet
{
    [ShowInInspector]
    List<SerializableExArg> targetArgs { get { return _targetArgs; } set { _targetArgs = value; } }
    //これがSalvageTime中にすべて起こった場合のみ発行される
    [SerializeField, HideInInspector] List<SerializableExArg> _targetArgs;
    [SerializeField] LogData log;

    public override LogData Trigger(ExploreArg arg)
    {
        for(int i = 0;i < targetArgs.Count;i++)
        {
            if(arg.Equals(targetArgs[i]))
            {
                targetArgs.Remove(targetArgs[i]);
            }
        }

        return null;
    }

    public override SalvagetimeExArgAndSDataset<LogData> Clone()
    {
        var clone = MemberwiseClone() as StepExploreUniqueLogData;

        clone._targetArgs = new List<SerializableExArg>();

        //Listからメンバーを差っ引くので、ここはディープにコピー
        for (int i = 0; i < _targetArgs.Count; i++)
        {
            clone._targetArgs.Add(_targetArgs[i]);
        }

        return clone;
    }
}

