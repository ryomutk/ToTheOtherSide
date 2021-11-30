using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class GameSessionData:ISalvageData
{
    public SectorMap map{get;set;}
    public MOTHERData.MotherEntity MOTHER{get;set;}
    public Vector2 currentMOTHERCoordinate{get;set;}
    public bool stockIsFull{get{return stocks.Count >= MOTHER.GetStatus(StatusType.stock);}}
    //現在の資源データ
    public Dictionary<ItemID,int> resourceTable = new Dictionary<ItemID, int>();
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