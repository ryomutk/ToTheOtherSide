using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class TouchPanel : MonoBehaviour
{
        Button entityButton;    

    protected virtual void Start()
    {
        entityButton = GetComponent<Button>();

        entityButton.onClick.AddListener(()=>
        {
            ScreenTouchArg arg;
            #if UNITY_EDITOR || UNITY_STANDALONE
            arg = new ScreenTouchArg(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            #endif
            #if UNITY_IPHONE || UNITY_ANDROID
            arg = new ScreenTouchArg(Camera.main.ScreenToWorldPoint(Input.touches[0].position))
            #endif

            OnClick(arg);
        });
    }

    public abstract void OnClick(ScreenTouchArg arg);
}