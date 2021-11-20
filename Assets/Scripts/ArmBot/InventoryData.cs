using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections.ObjectModel;
using System.Linq;

[Serializable]
public class InventoryData:ISalvageData
{
    public ReadOnlyCollection<ItemID> items{get{return _items.AsReadOnly();}}
    [SerializeField] List<ItemID> _items;

    public bool Add(ItemID data)
    {
        _items.Add(data);
        return true;
    }

    public bool Remove(ItemID data)
    {
        return _items.Remove(data);
    }
}