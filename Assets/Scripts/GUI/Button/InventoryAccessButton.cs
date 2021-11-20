using UnityEngine;

//InvDataに対する処理を行えるように拡張した
//DataImportButton
public class InventoryAccessButton:DataImportButton
{
    enum InventoryAction
    {
        add,
        remove
    }
    [SerializeField] InventoryAction method;
    protected override void OnClick()
    {
        var strage = toData.GetData<ItemStorage>(0);
        var item = fromData.GetData<SalvageValuable<ItemID>>(0).value;
        if(method == InventoryAction.add)
        {
            strage.inventory.Add(item);
        }
        else if(method == InventoryAction.remove)
        {
            strage.inventory.Remove(item);
        }

    }
}