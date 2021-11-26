using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class ButtonField : DataImportField, IEventListener<SystemEventArg>
{
    protected override AssetLabelReference loadKey { get { return choiceKey; } }
    [SerializeField] Transform buttonPlace;
    protected InstantPool<GUIControllButton> buttonPool;
    [SerializeField] GUIControllButton buttonPref;
    [SerializeField] int prepareNum = 10;

    [SerializeField] AssetLabelReference choiceKey;

    //Selectedを伝えるためのScriptableのLabel
    [SerializeField] AssetLabelReference selectedKey;
    DataIndexer selected;
    protected Dictionary<GUIControllButton, ISalvageData> choices = new Dictionary<GUIControllButton, ISalvageData>();



    void Start()
    {
        EventManager.instance.Register(this, EventName.SystemEvent);
    }

    public ITask OnNotice(SystemEventArg arg)
    {
        if (arg.state == GameState.ViewInitialize)
        {
            buttonPool = new InstantPool<GUIControllButton>(buttonPlace);
            buttonPool.CreatePool(buttonPref, prepareNum, false);

            buttonPool.ForeachObject(
                (button) => button.baseButton.onClick.AddListener(() => OnClick(button)));


            EventManager.instance.Disregister(this, EventName.SystemEvent);
        }
        return SmallTask.nullTask;
    }

    void OnClick(GUIControllButton button)
    {
        var data = choices[button];
        //複数選択未実装
        selected.GetData<SalvageValuable<ISalvageData>>(0).value = data;

#if UNITY_EDITOR
        Debug.Log("Data Set");
#endif
    }

    protected override IEnumerator LoadRoutine(SmallTask task)
    {
        var loadTask = DataManager.LoadDatasAsync(selectedKey);

        yield return new WaitUntil(() => loadTask.compleated);
        selected = loadTask.result;

        StartCoroutine(base.LoadRoutine(task));
    }

    protected override void SetField()
    {
        choices.Clear();
        buttonPool.DisableAll();

        if (entity == null)
        {
            Debug.LogWarning("datas may not be in suitable type");
        }

        var datas = ExtractIndexer();

        foreach (var data in datas)
        {
            var button = buttonPool.GetObj();
            choices[button] = data;

            if (buttonPref.iconField != null)
            {
                button.iconField.LoadData(data);
            }
        }
    }

    //並べるボタンを抽出するメソッド。
    //拡張先でいろんな要素を並べられるように分離。
    protected virtual DataIndexer ExtractIndexer()
    {
        //デフォルトでのデータのやり取りはSValを介している前提のやつ
        return entity.GetData<SalvageValuable<ISalvageData>>(0).value as DataIndexer;
    }

}