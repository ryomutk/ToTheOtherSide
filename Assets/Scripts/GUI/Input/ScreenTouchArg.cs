using System;
using UnityEngine;

//画面をタッチしたときに発行されるイベント
//イベントでは使えなくした
public class  ScreenTouchArg
{
    public Vector2 worldPosition{get;} 
    
    public ScreenTouchArg(Vector2 coordinate)
    {
        this.worldPosition = coordinate;
    }
}