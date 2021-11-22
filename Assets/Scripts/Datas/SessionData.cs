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
    public float nowDepth { get; private set; }
    public float maxDepth { get; private set; }
    [ShowInInspector,ReadOnly]
    public List<SerializableExArg> eventsOccoured{get;private set;}
    public List<int> visitedPlace{get;private set;}
    public int nowPlace{get;private set;}
    //exArgは投げない。変更に対応しずらくなるので。
    public event Action onUpdate;

    SessionState nowState;
    enum SessionState
    {
        none,
        onGoing,
        compleated
    }


    public bool OnNotice(ExploreArg arg)
    {
        if (arg.type == ExploreObjType.Interact)
        {
            #if UNITY_EDITOR
            Debug.Log("nowDepth:"+nowDepth);
            #endif
            nowDepth += (arg as StepActionArg).depthDelta;
            if (nowDepth > maxDepth)
            {
                maxDepth = nowDepth;
            }
        }
        else if(arg is StepExArg sarg)
        {
            if(!visitedPlace.Contains(sarg.step))
            {
                visitedPlace.Add(sarg.step);
            }
            nowPlace = sarg.step;
        }

        eventsOccoured.Add(arg as SerializableExArg);

        if(onUpdate!=null)
        {
            onUpdate();
        }

        return true;
    }

    public SessionData()
    {
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