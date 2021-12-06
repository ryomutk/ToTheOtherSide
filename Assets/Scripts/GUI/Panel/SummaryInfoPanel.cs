using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class SummaryInfoPanel:UIPanel,ILoadSalvageData<SessionData>
{
    public override PanelName name => PanelName.SummaryPanel;
    [SerializeField] TMP_Text distanceField;
    [SerializeField] TMP_Text durationField; 
    [SerializeField] TMP_Text nameField;
    [SerializeField] TMP_Text placeMiasma;
    [SerializeField] TMP_Text barrier;

    public ITask Load(SessionData data)
    {
        if(data.visitedPlace.Count==0)
        {
            #if DEBUG
            Debug.LogWarning("no island found!");
            #endif
            return SmallTask.nullTask;
        }
        var island = data.visitedPlace[0];
        var metaIsland = IslandUtility.GetMetaData(island);
        distanceField.text = metaIsland.metorDistanceFM.ToString();
        durationField.text = SessionConfig.instance.GetSessionDuration(data).ToString();
        nameField.text = metaIsland.name;
        placeMiasma.text = metaIsland.miasma.ToString();
        barrier.text = metaIsland.barrier.ToString();

        return Show();
    }
    
    
    protected override ITask Show()
    {
        return base.Show();
    }

    protected override ITask Hide()
    {
        return base.Hide();
    }
}