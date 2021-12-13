using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using System;
public class GameSessionData:ISalvageData
{
    public SectorMap map{get;set;}
    [ShowInInspector,ReadOnly]
    public MOTHERData.Entity MOTHER{get;set;}
    [ShowInInspector,ReadOnly]
    public Vector2 currentMOTHERCoordinate{get;set;}
    [ShowInInspector,ReadOnly]
    public bool stockIsFull{get{return stocks.Count >= MOTHER.GetStatus(StatusType.stock);}}
    //現在の資源データ
    [ShowInInspector,ReadOnly]
    public Dictionary<ItemID,int> resourceTable = new Dictionary<ItemID, int>();
    [ShowInInspector,ReadOnly]
    public ReadOnlyCollection<ArmBotData.Entity> stocks{get{return _stocks.AsReadOnly();}}
    List<ArmBotData.Entity> _stocks = new List<ArmBotData.Entity>();
    
    public bool AddStock(ArmBotData.Entity entity)
    {
        if(!stockIsFull)
        {
            _stocks.Add(entity);
            return true;
        }
        return false;
    }

}