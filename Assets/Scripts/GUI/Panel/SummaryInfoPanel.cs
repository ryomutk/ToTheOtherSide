using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class SummaryInfoPanel:UIPanel,ILoadSalvageData<SessionData>
{
    public override PanelName name => PanelName.SummaryPanel;
    [SerializeField] TMP_Text distanceField;
    [SerializeField] TMP_Text resourceField;
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

        var island = data.visitedPlace[data.visitedPlace.Count-1];
        var metaIsland = MapUtility.GetMetaData(island);
        distanceField.text = metaIsland.metorDistanceFM.ToString();
        var durationSec = SessionConfig.instance.GetSessionDuration(data);
        var min = (int)durationSec/60;
        durationField.text = ":"+durationSec.ToString()+"m"+(durationSec-min*60).ToString()+"s";
        nameField.text = metaIsland.name;
        placeMiasma.text = metaIsland.miasma.ToString();
        barrier.text = metaIsland.barrier.ToString();
        resourceField.text = metaIsland.resource.ToString();
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