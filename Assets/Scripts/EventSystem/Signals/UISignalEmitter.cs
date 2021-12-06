using UnityEngine;
using System;
using UnityEngine.UI;

public class UISignalEmitter : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if(button!=null)
        {
            button.onClick.AddListener(()=>Emit());
        }
    }

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