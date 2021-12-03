using UnityEngine;

[RequireComponent(typeof(IUIRenderer))]
public abstract class UIPanel : MonoBehaviour, IEventListener<UIEventArg>
{
    new IUIRenderer renderer;
    new public abstract PanelName name { get; }
    bool showing = false;

    protected virtual void Start()
    {
        renderer = GetComponent<IUIRenderer>();
        EventManager.instance.Register(this,EventName.UIEvent);
    }

    public ITask OnNotice(UIEventArg arg)
    {
        if (arg.name.HasFlag(name))
        {
            if(arg.method == PanelAction.hide&&showing)
            {
                showing = false;
                return Hide();
            }
            else if(arg.method == PanelAction.show&&!showing)
            {
                showing = true;
                return Show();
            }
        }

        return SmallTask.nullTask;
    }

    protected virtual ITask Show()
    {
        return renderer.Draw();
    }

    protected virtual ITask Hide()
    {
        return renderer.Hide();
    }

    protected virtual void OnDisable()
    {
        EventManager.instance.Disregister(this,EventName.UIEvent);
    }
}