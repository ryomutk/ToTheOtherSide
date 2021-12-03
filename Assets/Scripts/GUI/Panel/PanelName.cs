using System;
[Flags]
public enum PanelName
{
    none = 0,
    StockPanel = 1,
    SideBar = 1 << 1,
    BackPanel = 1 << 2
}