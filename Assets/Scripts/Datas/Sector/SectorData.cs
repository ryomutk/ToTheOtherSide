using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using System.Linq;

//セクターの情報を保持し、保存する
public class SectorData:ScriptableObject,ISalvageData
{   
    new public string name{get{return _name;}}
    public SectorStep.StepBehaviourBase behaviour{get{return _behaviour;}}
    public SectorStepData rootStep{get{return _rootStep;}}
    public SectorStepData lastStep{get{return _lastStep;}}
    public int sectorNum{get{return _sectorNum;}}
    [SerializeField] string _name = "New Sector";
    //StepBehaviourの受けですべての継承クラスがシリアライズできるようにするすべを模索宇
    [SerializeField] StepBehaviourPrototype _behaviour;
    [SerializeField] SectorStepData _rootStep;
    [SerializeField] SectorStepData _lastStep;
    [SerializeField] int _sectorNum = 0;
    
    void OnValidate()
    {
        _rootStep._alignSector = sectorNum;
    }

    public bool SetRoot(SectorStepData root)
    {
        if(_rootStep == null)
        {
            _rootStep = root;
            return true;
        }

        return false;
    }
    
}