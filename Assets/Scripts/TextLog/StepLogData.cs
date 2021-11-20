using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class StepLogData
{
    [HideInInspector] public SectorStepData parent;
    [EnumToggleButtons]public InteractType interactType;
    [HideLabel]public LogData log;

    public enum InteractType
    {
        discover,
        entry,
        exit
    }
}