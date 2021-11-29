using System.Collections.Generic;
using UnityEngine;

public static class  SessionUtility
{
    public static ITask<SessionData> RequestSummary(BotType type,Vector2Int startCoords,Vector2 direction)
    {
        var entity = ArmBotData.CreateInstance(type,direction);
        return SessionManager.instance.RequestSummary(entity,startCoords);
    }

    public static bool RequestSession(SessionData data)
    {
        SessionManager.instance.ExecuteSession(data);
        return true;
    }
}