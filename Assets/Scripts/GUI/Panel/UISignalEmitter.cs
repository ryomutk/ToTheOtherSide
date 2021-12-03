using UnityEngine;
using System;

public class UISignalEmitter : MonoBehaviour
{
    [Serializable]
    class UIActions
    {
        [SerializeField]public PanelName names;
        [SerializeField]public ShowType type;
        [SerializeField]public PanelAction action;
    }

    [SerializeField] UIActions[] actions;

    public void Emit()
    {
        for (int i = 0; i < actions.Length; i++)
        {
            var arg = new UIEventArg(actions[i].names,actions[i].type,actions[i].action);
            EventManager.instance.Notice(EventName.UIEvent, arg);
        }
    }
}