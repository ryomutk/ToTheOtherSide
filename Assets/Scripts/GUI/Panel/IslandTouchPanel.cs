using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class IslandTouchPanel : MonoBehaviour, IEventListener<ScreenTouchArg>, IEventListener<SystemEventArg>
{
    [SerializeField] RectTransform targetTransform;

    [SerializeField] GameState[] targetStates;

    //入力を受けるためのList
    List<Island> sectorlist = new List<Island>();
    StringBuilder logString = new StringBuilder();

    void Start()
    {
        EventManager.instance.Register<SystemEventArg>(this,EventName.SystemEvent);
    }

    public ITask OnNotice(SystemEventArg arg)
    {
        if (targetStates.Contains(arg.state))
        {
            return EventManager.instance.Register<ScreenTouchArg>(this,EventName.ScreenTouchEvent);
        }
        else
        {
            EventManager.instance.Disregister<ScreenTouchArg>(this,EventName.ScreenTouchEvent);
            return SmallTask.nullTask;
        }
    }

    public ITask OnNotice(ScreenTouchArg arg)
    {
        var localPosition = targetTransform.InverseTransformPoint(arg.worldPosition);

        Vector2 coordinate = localPosition / StepGenerationConfig.instance.gridToCanvasrate;
        coordinate += StepGenerationConfig.instance.originCoords;
        var res = DataProvider.nowGameData.map.TryFindRange(coordinate, 0, ref sectorlist);

        if (res == 1)
        {
            var step = sectorlist[0];
            var selectableArg = new SelectableArg(step);
            return EventManager.instance.Notice(EventName.SelectableEvent,selectableArg);
        }

#if DEBUG
        logString.Clear();
        logString.AppendLine("====TOUCH:ISLAND====");
        logString.Append("WorldPos:");
        logString.AppendLine(arg.worldPosition.ToString());
        logString.Append("LocalPosition:");
        logString.AppendLine(localPosition.ToString());
        logString.Append("   coordinate:");
        logString.AppendLine(coordinate.ToString());
        if (res == 1)
        {
            logString.AppendLine("Step Found");
        }
        else if (res > 1)
        {
            logString.AppendLine("Too many Error");
        }
        else
        {
            logString.AppendLine("Step not found");
        }

        logString.AppendLine("\n\n\n");
        Utility.LogWriter.Log(logString.ToString(), "EventLog", true);
#endif


        return SmallTask.nullTask;
    }
}