using UnityEngine;
//配置はIslandMapの下でよろしく
public abstract class IslandMapAddon:UIPanel
{
    protected IslandMapPanel mapPanel;
    protected override void Start()
    {
        base.Start();
        mapPanel = GetComponentInParent<IslandMapPanel>();
    }
}