using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// SalvageValuable<T>を読み込むField
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoDataField<T> : DataImportField
where T : ISalvageData
{
    protected override AssetLabelReference loadKey { get { return _loadKey; } }
    [SerializeField] AssetLabelReference _loadKey;
    [SerializeField] bool updateRealtime;
    SalvageValuable<ISalvageData> rawData { get { return (SalvageValuable<ISalvageData>)entity[0]; } }
    protected T data { get { return (T)rawData.value; } }

    //event登録＆解除のためのやつ
    Action<ISalvageData> drawOnUpdate;

    protected virtual void Start()
    {
        drawOnUpdate = (x) => DrawField(data);
    }

    protected override void SetField()
    {
        if (updateRealtime)
        {
            rawData.onValueChanged += drawOnUpdate;
        }
        DrawField((T)rawData.value);
    }


    protected abstract void DrawField(T data);

    public override void UnloadData()
    {
        if (updateRealtime)
        {
            rawData.onValueChanged -= drawOnUpdate;
        }
        base.UnloadData();
    }
}