using UnityEngine;

public class UISignalEmitter:MonoBehaviour
{
    [SerializeField] PanelName names;
    [SerializeField] ShowType type;
    [SerializeField] PanelAction action;
    public void Emit()
    {
        var arg = new UIEventArg(names,type,action);
        EventManager.instance.Notice(EventName.UIEvent,arg);
    }
}