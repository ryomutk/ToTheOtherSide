using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackPanel:UIPanel
{
    Button button;
    public override PanelName name{get{return PanelName.BackPanel;}}
    [SerializeField] PanelName ignorePanel;

    protected override void Start()
    {
        base.Start();
        button = GetComponent<Button>();
        button.onClick.AddListener(Signal);
    }

    void Signal()
    {
        PanelName allNames = 0;
        allNames = ~allNames;
        allNames -= ignorePanel;
        var arg = new UIEventArg(allNames,0,PanelAction.hide);
        EventManager.instance.Notice(EventName.UIEvent,arg);
    }

}