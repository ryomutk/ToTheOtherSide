using System.Collections.Generic;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class ExploreLogDataSets : ExploreAndSDataSets<LogData>
{
    protected override List<SalvagetimeExArgAndSDataset<LogData>> callbackDatas
    {
        get
        {
            if(_callbacks == null)
            {
                Initialize();
            }

            return _callbacks;
        }
    }
    List<SalvagetimeExArgAndSDataset<LogData>> _callbacks;

    [ShowInInspector] List<SerializableExploreAndLogDataSet> dataSets{get{return _dataSets;} set{_dataSets = value;}}
    [SerializeField, HideInInspector] List<SerializableExploreAndLogDataSet> _dataSets;

    void Initialize()
    {
        _callbacks = new List<SalvagetimeExArgAndSDataset<LogData>>();

        for(int i = 0;i< _dataSets.Count;i++)
        {
            _callbacks.Add(_dataSets[i]);
        }
    }
}