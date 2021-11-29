using System;
using UnityEngine;

//画面をタッチしたときに発行されるイベント
public class  ScreenTouchArg:SalvageEventArg
{
    public Vector2 worldPosition{get;} 
    
    public ScreenTouchArg(Vector2 coordinate)
    {
        this.worldPosition = coordinate;
    }
}