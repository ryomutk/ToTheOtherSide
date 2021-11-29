using UnityEngine;

public class UIEventArg:SalvageEventArg
{
    public PanelName name{get;}
    public ShowType type{get;}
    public PanelAction method{get;}

    public UIEventArg(PanelName name,ShowType type,PanelAction method)
    {
        this.name = name;
        this.type = type;
        this.method = method;
    } 
}

public enum ShowType
{
    overrap,
    swap
}

public enum PanelAction
{
    show,
    hide
}