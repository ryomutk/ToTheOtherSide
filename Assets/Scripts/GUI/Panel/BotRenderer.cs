using UnityEngine;
using System.Collections.Generic;

public class BotRenderer : MonoBehaviour, IUIRenderer, IEventListener<ExploreArg>, IEventListener<SessionEventArg>
{
    //[SerializeField] SerializableDictionary<BotType,BotObject> icons;
    //テスト用にアイコンは一つ
    [SerializeField] BotObject icon;
    [SerializeField] Transform mapOrigin;
    InstantPool<BotObject> botPool;
    Dictionary<ArmBotData.Entity, BotObject> iconDictionary = new Dictionary<ArmBotData.Entity, BotObject>();
    bool drawing = false;
    void Start()
    {
        EventManager.instance.Register<ExploreArg>(this, EventName.RealtimeExploreEvent);
        EventManager.instance.Register<SessionEventArg>(this, EventName.SessionEvent);
        botPool = new InstantPool<BotObject>(mapOrigin);
    }

    public ITask OnNotice(SessionEventArg arg)
    {
        if (!drawing)
        {
            return SmallTask.nullTask;
        }

        if (arg.state == SessionState.start)
        {
            var obj = botPool.GetObj();
            iconDictionary[arg.data.master] = obj;
            obj.transform.localPosition = Vector2.zero;
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
        if (!drawing)
        {
            return SmallTask.nullTask;
        }


        if (arg is TravelExArg trarg)
        {
            //座標を上書きするだけ
            iconDictionary[arg.from].transform.localPosition = trarg.coordinate * StepGenerationConfig.instance.gridToCanvasrate;
        }

        return SmallTask.nullTask;
    }

    public ITask Draw()
    {
        drawing = true;
        return SmallTask.nullTask;
    }

    public ITask Hide()
    {
        drawing = false;
        return SmallTask.nullTask;
    }
}