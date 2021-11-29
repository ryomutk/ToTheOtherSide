using UnityEngine;

public class TouchScreen : MonoBehaviour
{
    void Update()
    {
        #if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            var arg = new ScreenTouchArg(touch.position);

            EventManager.instance.Notice(EventName.ScreenTouchEvent, arg);
        }
        #endif
        
        #if UNITY_WEBGL || UNITY_STANDALONE || UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            var arg = new ScreenTouchArg(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            EventManager.instance.Notice(EventName.ScreenTouchEvent,arg);
        }
        #endif
    }
}