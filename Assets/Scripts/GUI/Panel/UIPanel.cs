using UnityEngine;

[RequireComponent(typeof(IUIRenderer))]
public abstract class UIPanel : MonoBehaviour, IEventListener<UIEventArg>
{
    new IUIRenderer renderer;
    new public abstract PanelName name { get; }
    bool showing = false;
    [SerializeField] UIPanel[] childrenPanels;
    //Trueの場合,ChildrenPanelの一番上のものをShowで見せる。
    //Falseの場合、すべてHideする
    [SerializeField] bool showDefault;

    protected virtual void Start()
    {
        renderer = GetComponent<IUIRenderer>();
        EventManager.instance.Register(this, EventName.UIEvent);
    }

    public ITask OnNotice(UIEventArg arg)
    {
        if (arg.name.HasFlag(name))
        {
            if (arg.method == PanelAction.hide && showing)
            {
                showing = false;
                return Hide();
            }
            else if (arg.method == PanelAction.show && !showing)
            {
                showing = true;
                return Show();
            }
        }

        return SmallTask.nullTask;
    }

    protected virtual ITask Show()
    {
        if (showDefault && childrenPanels.Length != 0)
        {
            childrenPanels[0].Show();

            for (int i = 1; i < childrenPanels.Length; i++)
            {
                childrenPanels[i].Hide();
            }
        }
        return renderer.Draw();
    }

    protected virtual ITask Hide()
    {
        return renderer.Hide();
    }

    protected virtual void OnDisable()
    {
        EventManager.instance.Disregister(this, EventName.UIEvent);
    }
}