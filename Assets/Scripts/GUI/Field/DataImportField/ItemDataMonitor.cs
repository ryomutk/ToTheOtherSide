using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;

//データを一つ、表示する場所
[RequireComponent(typeof(GUIPanel))]
public class ItemDataMonitor : MonoDataField<ItemData>
{
    [SerializeField] TMP_Text  nameField;
    [SerializeField] TMP_Text discriptionField;
    [SerializeField] Image thumbnailField;

    protected override void DrawField(ItemData data)
    {
        nameField.text = data.name;
        discriptionField.text = data.discription;
        thumbnailField.sprite = data.thumbNail;
    }
}