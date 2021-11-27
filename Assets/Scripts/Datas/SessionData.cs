using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;


/// <summary>
/// セッションのデータ
/// SalValで今のSessionをやり取りしたり、するためのやつ
/// Serializableなのでどっかに保存しとくこともできる
/// </summary>
[Serializable]
public class SessionData : ISalvageData, IEventListener<ExploreArg>
{
    public bool isFinished { get { return nowState == SessionState.compleated; } }
    public ArmBotData.Entity master { get; }

    public float nowHp { get; private set; }
    [ShowInInspector, ReadOnly]
    public List<SerializableExArg> eventsOccoured { get; private set; }
    public List<int> visitedPlace { get; private set; }
    public int nowPlace { get; private set; }
    //exArgは投げない。変更に対応しずらくなるので。
    public event Action onUpdate;
    public Vector2 nowCoordinate{get;private set;}

    SessionState nowState;
    enum SessionState
    {
        none,
        onGoing,
        compleated
    }


    public ITask OnNotice(ExploreArg arg)
    {
        if (arg.from == master)
        {

            if (arg.type == ExploreObjType.Travel)
            {
#if DEBUG
                Debug.Log("nowCoordinate" + nowCoordinate);
#endif
                nowCoordinate += (arg as TravelExArg).coordinate;
            }
            else if (arg is StepExArg sarg)
            {
                if (!visitedPlace.Contains(sarg.step))
                {
                    visitedPlace.Add(sarg.step);
                }
                nowPlace = sarg.step;
            }

            eventsOccoured.Add(arg as SerializableExArg);

            if (onUpdate != null)
            {
                onUpdate();
            }
        }

        return SmallTask.nullTask;
    }

    public SessionData(ArmBotData.Entity wingMan)
    {
        this.master = wingMan;
        visitedPlace = new List<int>();
        eventsOccoured = new List<SerializableExArg>();
        nowState = SessionState.onGoing;
    }

    public void Compleated()
    {
        nowState = SessionState.compleated;
        onUpdate = null;
    }
}