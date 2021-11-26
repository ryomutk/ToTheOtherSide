using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using TMPro;
using System.Collections;

//受け取ったメッセージを表示するフィールド
public abstract class TextField : MonoBehaviour
{
    [Flags]
    protected enum StateFlag : int
    {
        none = 0,
        playing = 1,
        fastForward = 2
    }
    int bitFlag;

    public bool playing { get { return flag.HasFlag(StateFlag.playing); } }

    protected StateFlag flag { get { return (StateFlag)bitFlag; } }


    public SmallTask PlayMessage(string message)
    {
        if (!flag.HasFlag(StateFlag.playing))
        {
            var task = new SmallTask();

            StartCoroutine(MessageRoutine(message, task));
            return task;
        }

        return null;
    }

    //瞬時に終わらせる
    public void Compleate()
    {
        if (!flag.HasFlag(StateFlag.fastForward))
        {
            StartCoroutine(CompleateMotion());
        }
    }

    IEnumerator CompleateRoutine()
    {
        bitFlag += (int)StateFlag.fastForward;

        yield return StartCoroutine(CompleateMotion());

        bitFlag -= (int)StateFlag.fastForward;
    }

    IEnumerator MessageRoutine(string message, SmallTask task)
    {
        bitFlag += (int)StateFlag.playing;

        yield return StartCoroutine(MessageMotion(message));

        task.compleated = true;

        bitFlag -= (int)StateFlag.playing;
    }

    protected abstract IEnumerator CompleateMotion();

    protected abstract IEnumerator MessageMotion(string message);
}