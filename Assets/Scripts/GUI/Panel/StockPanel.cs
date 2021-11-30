using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StockPanel : MonoBehaviour, IUIPanel
{
    new public PanelName name { get { return PanelName.StockPanel; } }
    [SerializeField] TMP_Text nowCount;
    [SerializeField] TMP_Text maxNum;
    [SerializeField] StockInochiButton buttonPref;
    InstantPool<StockInochiButton> buttonPool;
    [SerializeField] int initNum;
    [SerializeField] Transform buttonArea;

    void Start()
    {
        buttonPool = new InstantPool<StockInochiButton>(buttonArea);
        buttonPool.CreatePool(buttonPref, initNum, false);
    }

    public ITask Show()
    {
        foreach (var stock in DataProvider.nowGameData.stocks)
        {
            var button = buttonPool.GetObj();
            button.Initialize(stock);
        }
        var task = new SmallTask();

        StartCoroutine(DrawRoutine(task));

        return task;
    }

    IEnumerator DrawRoutine(SmallTask task)
    {
        List<ITask> tasks = new List<ITask>();
        buttonPool.ForeachObject(x =>
        {
            var task = x.renderer.Draw();
            tasks.Add(task);
        });

        for (int i = 0; i < tasks.Count; i++)
        {
            yield return new WaitUntil(() => tasks[i].compleated);
        }

        task.compleated = true;
    }

    public ITask Hide()
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
            var task = x.renderer.Hide();
            tasks.Add(task);
        });

        for (int i = 0; i < tasks.Count; i++)
        {
            yield return new WaitUntil(() => tasks[i].compleated);
        }

        buttonPool.ForeachObject(x=>x.gameObject.SetActive(false));
        task.compleated = true;
    }
}