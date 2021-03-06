using UnityEngine;
using Sirenix.OdinInspector;


[RequireComponent(typeof(IUIRenderer))]
public abstract class UIPanel : MonoBehaviour, IEventListener<UIEventArg>
{
    new IUIRenderer renderer;
    new public abstract PanelName name { get; }
    protected bool showing { get; private set; }
    [SerializeField] UIPanel[] childrenPanels;

    //warningを表示するかの判定のみに使う
    bool showCollision{get{return showDefault&&showAll;}}
    //Trueの場合,ChildrenPanelの一番上のものをShowで見せる。
    //Falseの場合、すべてHideする
    //ShowAll優先。
    [InfoBox("Show All優先です",InfoMessageType.Warning,"showCollision")]
    [SerializeField,ShowIf("@childrenPanels.Length > 0")] bool showDefault = false;
    [SerializeField,ShowIf("@childrenPanels.Length > 0")] bool swapChild = false;
    //Parentと同時にすべて表示する。
    [SerializeField,ShowIf("@childrenPanels.Length > 0")] bool showAll = false;

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
        //システムなどでこれを使ったりする時を考えてここで模範店至徳
        showing = true;
        if(showAll && childrenPanels.Length!=0)
        {
            for(int i = 0;i<childrenPanels.Length;i++)
            {
                childrenPanels[i].Show();
            }
        }
        else if (showDefault && childrenPanels.Length != 0)
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
        //システムなどでこれを使ったりする時を考えてここで模範店至徳
        showing = false;

        return renderer.Hide();
    }

    protected virtual void OnDisable()
    {
        EventManager.instance.Disregister(this, EventName.UIEvent);
    }
}