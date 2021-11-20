using System;


[Flags]
public enum PanelName
{
    home = 0,
    SessionInfo     = 1,
    SessionSummary  = 2,
    SessionResult   = 4,
    Inventory       = 8,
    SeaInfoPanel    = 16,
    ItemInfoPanel   = 32,
    SystemConfig    = 64,
    BackButtonPanel = 128,
    EquipmentPanel  = 256,
    EquipmentBar    = 512,
    EquipmentChoice = 1024,
    ToolBar         = 2048,
    StepInfo        = 4096,
    ScreenButton    = 8192
}