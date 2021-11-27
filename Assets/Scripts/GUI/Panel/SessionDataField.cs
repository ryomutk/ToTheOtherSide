using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using TMPro;

public class SessionDataField : MonoDataField<SessionData>
{
    //Updateの時のやつ
    Action drawFieldMethod;
    [SerializeField] TMP_Text placeField = null;
    [SerializeField] TMP_Text placeNameField = null;

    protected override IEnumerator LoadRoutine(SmallTask task)
    {
        return base.LoadRoutine(task);
    }

    protected override void SetField()
    {
        if (data != null)
        {
            drawFieldMethod = () => DrawField(data);
            data.onUpdate += drawFieldMethod;
        }
        base.SetField();
    }

    public override void UnloadData()
    {
        if (data != null)
        {
            data.onUpdate -= drawFieldMethod;
        }
        base.UnloadData();
    }


    protected override void DrawField(SessionData data)
    {
        //起動直後はsessionDataがnullの場合がある。
        if (data == null)
        {
            return;
        }

        if (placeField != null)
        {
            placeField.text = Mathf.Floor(data.nowPlace).ToString();
        }

        if (placeNameField != null)
        {
            placeNameField.text = data.nowPlace.ToString();
        }

    }
}