using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChatArg
{
    public int targetField{get{return _targetField;}}
    public string message{get{return _message;}}
    [SerializeField] int _targetField;
    [SerializeField,Multiline] string _message;

}