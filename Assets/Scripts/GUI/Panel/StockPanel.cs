using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(IUIRenderer))]
public class StockPanel : UIPanel
{
    public override PanelName name { get { return PanelName.StockPanel; } }
    [SerializeField] TMP_Text nowCount;
    [SerializeField] TMP_Text maxNum;
    [SerializeField] StockInochiButton buttonPref;
    InstantPool<StockInochiButton> buttonPool;
    [SerializeField] int initNum;
    [SerializeField] ScrollRect buttonArea;
    [SerializeField] float buttonGap = 50f;

    protected override void Start()
    {
        base.Start();
        buttonPool = new InstantPool<StockInochiButton>(buttonArea.content);
        buttonPool.CreatePool(buttonPref, initNum, false);
    }



    protected override ITask Show()
    {
        foreach (var stock in DataProvider.nowGameData.stocks)
        {
            var button = buttonPool.GetObj();
            button.Initialize(stock);
        }
        var task = new SmallTask();

        var count = DataProvider.nowGameData.stocks.Count;
        var size = buttonArea.content.sizeDelta;
        size.y = ((RectTransform)buttonPref.transform).sizeDelta.y * count + buttonGap * count;
        buttonArea.content.sizeDelta = size;

        nowCount.text = DataProvider.nowGameData.stocks.Count.ToString();
        maxNum.text = DataProvider.nowGameData.MOTHER.GetStatus(StatusType.stock).ToString();

        StartCoroutine(DrawRoutine(task));

        return task;
    }

    IEnumerator DrawRoutine(SmallTask task)
    {
        var showTask = base.Show();
        yield return new WaitUntil(() => showTask.compleated);

        List<ITask> tasks = new List<ITask>();
        buttonPool.ForeachObject(x =>
        {
            if (x.isActiveAndEnabled)
            {
                var task = x.renderer.Draw();
                tasks.Add(task);
            }
        });

        for (int i = 0; i < tasks.Count; i++)
        {
            yield return new WaitUntil(() => tasks[i].compleated);
        }

        task.compleated = true;
    }

    protected override ITask Hide()
    {
        var task = new SmallTask();

        StartCoroutine(HideRoutine(task));

        return task;
    }

    IEnumerator HideRoutine(SmallTask task)
    {
        List<ITask> tasks = new List<ITask>();

        buttonPool.ForeachObject(x =>
        {
            if (x.isActiveAndEnabled)
            {
                var task = x.renderer.Hide();
                tasks.Add(task);
            }
        });

        for (int i = 0; i < tasks.Count; i++)
        {
            yield return new WaitUntil(() => tasks[i].compleated);
        }

        var showTask = base.Hide();
        yield return new WaitUntil(() => showTask.compleated);

        buttonPool.ForeachObject(x => x.gameObject.SetActive(false));
        task.compleated = true;
    }
}