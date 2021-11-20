using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomLayout : MonoBehaviour
{
    [SerializeField] Transform[] elementTransforms;
    [SerializeField, Tooltip("Correct children transforms automaticly")] bool autoCollectChild;
    int defaultChildCount;
    List<Transform> placedElements = new List<Transform>();

    protected virtual void Start()
    {
        GameManager.instance.OnSystemEvent += (x) =>
        {
            if (autoCollectChild)
            {
                elementTransforms = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    elementTransforms[i] = transform.GetChild(i);
                }
            }
            defaultChildCount = transform.childCount;

            var task = new SmallTask();
            task.ready = true;
            return task;
        };
    }

    bool AddElement(Transform element)
    {
        if (elementTransforms.Length > placedElements.Count)
        {
            var index = placedElements.Count;
            element.SetParent(elementTransforms[index]);
            placedElements.Add(element);

            return true;
        }

        Debug.Log("Your Place Is Full!");
        element.SetParent(transform);
        return false;
    }

    //配置中も呼ばれてしまうので、それを防ぐ。
    //フレーム単位ではなく、要素を動かすたびにコールバックとして呼ばれるようなので
    //ゆえに同時に2つ以上の要素をについて処理する必要がない。
    void OnTransformChildrenChanged()
    {
        var extra = transform.childCount - defaultChildCount;

        switch (extra)
        {
            //配置されたとき
            case 1:
                //追加されたものを取得
                var child = transform.GetChild(defaultChildCount);

                //追加
                AddElement(child);

                break;

            //だぶりのやつ
            case 0:
                break;

#if UNITY_EDITOR
            //多すぎるとき
            //安全のため二重チェック
            case int i when i > 1:
                Debug.Log("Too many children");
                break;

            //何？
            case int i when i < 0:
                Debug.LogWarning("Something went wrong extra:" + extra);
                break;
#endif
        }
    }
}