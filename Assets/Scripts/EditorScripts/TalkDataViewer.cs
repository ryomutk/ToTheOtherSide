#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class TalkDataViewer : ScriptableViewer<TalkData>
{
    protected override string targetPath { get { return _targetPath; } }
    [SerializeField] string _targetPath = "TalkData/";

    [MenuItem("Window/ScriptableViewer/TalkDataViewer")]
    static void OpenWindow()
    {
        var w = GetWindow<TalkDataViewer>("TalkDatas");
        w.Show();
        //w.LoadScriptables();
    }
}

#endif