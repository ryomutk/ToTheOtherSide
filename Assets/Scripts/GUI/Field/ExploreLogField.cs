using UnityEngine;

public class ExploreLogField:MonoBehaviour
{
    [SerializeField] TextField logPanelPref;
    [SerializeField] Transform panelPlace;
    [SerializeField] int initNum = 10;
    InstantPool<TextField> panelPool;


    protected void Start()
    {
        panelPool = new InstantPool<TextField>(panelPlace);
        panelPool.CreatePool(logPanelPref,initNum,false);
    }
    

    protected void SalTimeCallback(LogData arg)
    {
        var panel = panelPool.GetObj();
        panel.PlayMessage(arg.message);
    }
}