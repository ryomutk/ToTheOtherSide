using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

[Serializable]
public class LogData:ISalvageData
{
    [SerializeField,EnumToggleButtons]ArmLogType _type;
    [SerializeField,Multiline] string _message;

    ArmLogType type{get{return _type;}}
    public string message{get{return _message;} set{_message = value;}}
}