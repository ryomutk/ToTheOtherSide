#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

public class SessionInfoPanel : OdinEditorWindow
{
    [MenuItem("Window/SessionInfoPanel")]
    static void OpenWindow()
    {
        var w = GetWindow<SessionInfoPanel>("SessionInfo");
        w.Show();
    }

    [SerializeField] AssetLabelReference exploreEventLabel;
    [ShowInInspector, ReadOnly] ExploreEventBuilder eventBuilder;

    ExploreEvent exEvent;

    [Button]
    void Inspect()
    {
        if (eventBuilder == null)
        {
            var task = Addressables.LoadAssetAsync<ExploreEvent>(exploreEventLabel);

            task.Completed += (x) =>
            {
                exEvent = x.Result;
                eventBuilder = new ExploreEventBuilder(x.Result);
            };
        }
        else
        {
            Debug.LogWarning("AreadyStarted");
        }
    }

    [Button]
    void Stop()
    {
        if (eventBuilder != null)
        {
            Addressables.Release(exEvent);
            exEvent = null;
            eventBuilder = null;
        }
        else
        {
            Debug.LogWarning("not started");
        }
    }
}

#endif