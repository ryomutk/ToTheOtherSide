using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StepDataMonitor:MonoDataField<SectorStepData>
{
    [SerializeField] TMP_Text nameField;
    [SerializeField] Image thumbNail;
    [SerializeField] TMP_Text detailField;

    //使えるかテストするためにとりあえず作ったやつ
    protected override void DrawField(SectorStepData data)
    {
        nameField.text = data.name;
        thumbNail.sprite = data.thumbNail;
        detailField.text = data.detail;
    }
}