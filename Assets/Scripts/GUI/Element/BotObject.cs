using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//とりあえず単にアイコン的なのを想定してる
[RequireComponent(typeof(IUIRenderer))]
public class  BotObject:MonoBehaviour
{
    new IUIRenderer renderer;
    void Start()
    {
        renderer = GetComponent<IUIRenderer>();
    }

    public ITask Show()
    {
        return renderer.Draw();
    }

    public ITask Disable()
    {
        var task = renderer.Hide();
        StartCoroutine(Disable(task));
        return task;
    }

    IEnumerator Disable(ITask task)
    {
        yield return new WaitUntil(()=>task.compleated);
        gameObject.SetActive(false);
    }
}