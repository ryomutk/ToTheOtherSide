using UnityEngine;
using System.Collections.Generic;

//Nameがインスペクタから登録できて、動的部分のないパネル
[RequireComponent(typeof(IUIRenderer))]
public class  SimplePanel:UIPanel
{
    public override PanelName name{get{return _name;}}
    [SerializeField] PanelName _name;
}