using UnityEngine;

public class DataNameField:DataPreviewField
{
    [SerializeField] TMPro.TMP_Text nameField;

    /// <summary>
    /// 内部的にObjectに変換してるので注意。
    /// </summary>
    /// <param name="data"></param>
    public override void LoadData(ISalvageData data)
    {
        var obj = (Object)data;
        nameField.text = obj.name;
    }
}