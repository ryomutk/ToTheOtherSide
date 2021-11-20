#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SectorStepEditor : TreeStructuredDataEditor<SectorStepData>
{
    protected override string targetLabel { get { return "StepData/"; } }

    //[MenuItem("Window/SectorStepEditor")]
    static void OpenWindow()
    {
        var w = GetWindow<SectorStepEditor>("SectorStepEditor");
        w.Show();
    }
}

#endif