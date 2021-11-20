using UnityEngine;
using System.Collections;
using TMPro;

//一文字ずつ表示するタイプ。
public class TalkField : TextField
{
    //一文字毎の時間
    //継承先では使わなくなる可能性あり。
    [SerializeField] float letterDuration = 0.2f;
    [SerializeField] TMP_Text textField;


    protected override IEnumerator CompleateMotion()
    {

        var tmp = letterDuration;
        letterDuration = 0;
        yield return new WaitUntil(() => !flag.HasFlag(StateFlag.playing));
        letterDuration = tmp;
    }

    protected override IEnumerator MessageMotion(string message)
    {

        foreach (var letter in message)
        {
            textField.text += letter;
            yield return new WaitForSeconds(letterDuration);
        }


    }
}