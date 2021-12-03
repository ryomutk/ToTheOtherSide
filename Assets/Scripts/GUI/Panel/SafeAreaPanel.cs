using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SafeAreaPanel : UIPanel
{
    public override PanelName name{get{return PanelName.SafeAreaPanel;}}

    [SerializeField] TMP_Text resourceField;
    [SerializeField] TMP_Text cristalField;

    protected override ITask Show()
    {
        cristalField.text = DataProvider.nowGameData.resourceTable[ItemID.resource].ToString();
        resourceField.text = DataProvider.nowGameData.resourceTable[ItemID.cristal].ToString();

        return base.Show();
    }

    void Update()
    {
        //これTickでUpdateしたい。
        if (showing)
        {
            cristalField.text = DataProvider.nowGameData.resourceTable[ItemID.resource].ToString();
            resourceField.text = DataProvider.nowGameData.resourceTable[ItemID.cristal].ToString();
        }
    }

    protected override ITask Hide()
    {
        return base.Hide();
    }
}