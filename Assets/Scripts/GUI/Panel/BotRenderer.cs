using UnityEngine;
using System.Collections.Generic;

public class BotRenderer : UIPanel, IEventListener<ExploreArg>, IEventListener<SessionEventArg>
{
    public override PanelName name{get{return PanelName.BotRenderer;}}
    //[SerializeField] SerializableDictionary<BotType,BotObject> icons;
    //テスト用にアイコンは一つ
    [SerializeField] BotObject icon;
    [SerializeField] Transform mapOrigin;
    InstantPool<BotObject> botPool;
    Dictionary<ArmBotData.Entity, BotObject> iconDictionary = new Dictionary<ArmBotData.Entity, BotObject>();
    protected override void Start()
    {
        base.Start();

        EventManager.instance.Register<ExploreArg>(this, EventName.RealtimeExploreEvent);
        EventManager.instance.Register<SessionEventArg>(this, EventName.SessionEvent);
        botPool = new InstantPool<BotObject>(mapOrigin);
        botPool.CreatePool(icon,2,false);
    }

    public ITask OnNotice(SessionEventArg arg)
    {
        if (!showing)
        {
            return SmallTask.nullTask;
        }

        if (arg.state == SessionState.start)
        {
            var obj = botPool.GetObj();
            iconDictionary[arg.data.master] = obj;
            obj.transform.localPosition = (arg.data.startCoordinate-StepGenerationConfig.instance.originCoords)*StepGenerationConfig.instance.gridToCanvasrate;
            obj.Show();
        }
        else if (arg.state == SessionState.compleate)
        {
            var obj = iconDictionary[arg.data.master];

            //ここでGameobjectもDisableしてくれる安心。
            obj.Disable();
            iconDictionary.Remove(arg.data.master);
        }

        return SmallTask.nullTask;
    }

    public ITask OnNotice(ExploreArg arg)
    {
        if (!showing)
        {
            return SmallTask.nullTask;
        }


        if (arg is TravelExArg trarg)
        {
            //座標を上書きするだけ
            iconDictionary[arg.from].transform.localPosition += (Vector3) trarg.traveledVec * StepGenerationConfig.instance.gridToCanvasrate;
            iconDictionary[arg.from].Load(arg.from);
        }

        return SmallTask.nullTask;
    }

}