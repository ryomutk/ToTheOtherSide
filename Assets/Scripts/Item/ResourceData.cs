using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/Resources")]
public class ResourceData:ItemData
{
    public override ItemType type{get{return ItemType.resource;}}
    public override ItemAttribute attribute{get{return _attribute;}}
    public int amount{get{return _amount;}}
    [SerializeField] int  _amount;
}