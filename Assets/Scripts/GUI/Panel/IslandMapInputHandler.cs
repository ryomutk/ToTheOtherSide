using UnityEngine;
using UnityEngine.UI;

public class IslandMapInputHandler : IslandMapAddon,IEventListener<SelectableArg>
{
    public override PanelName name{get{return PanelName.MapInput;}}
    [SerializeField] Slider scaleSlider;

    public ITask OnNotice(SelectableArg arg)
    {
        if(showing&&arg.data is Island island)
        {
            scaleSlider.normalizedValue = 1;
            return mapPanel.Focus(island);
        }

        return SmallTask.nullTask;
    }

    protected override void Start()
    {
        scaleSlider.onValueChanged.AddListener((x) =>Zoom(x));
        base.Start();
    }

    void Zoom(float zoomRate)
    {
        mapPanel.Zoom(zoomRate);
    }

    protected override ITask Show()
    {
        EventManager.instance.Register<SelectableArg>(this,EventName.SelectableEvent);
        return base.Show();
    }

    protected override ITask Hide()
    {
        EventManager.instance.Disregister<SelectableArg>(this,EventName.SelectableEvent);
        return base.Hide();
    }
}