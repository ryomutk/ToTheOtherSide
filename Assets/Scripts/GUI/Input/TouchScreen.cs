using UnityEngine;

public class TouchScreen:MonoBehaviour
{
    void Update()
    {
        var touch = Input.GetTouch(0);
        var arg = new ScreenTouchArg(touch.position);
        
        EventManager.instance.Notice(EventName.ScreenTouchEvent,arg);
    }
}