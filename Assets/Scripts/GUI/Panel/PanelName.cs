using System;
[Flags]
public enum PanelName
{
    none = 0,
    all = ~0,
    StockPanel = 1,
    SideBar = 1 << 1,
    BackPanel = 1 << 2,
    MainButtons = 1 << 3,
    InochiButtons = 1 << 4,
    SafeAreaPanel = 1 << 5,
    IslandPanel = 1 << 6
}