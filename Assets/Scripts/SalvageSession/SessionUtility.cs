using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SessionUtility
{
    static int GetCost(BotType type)
    {
        return InochiConfig.GetPurchaseCost(type);
    }

    public static bool TryCreateNewInochi(BotType type, out ArmBotData.Entity entity)
    {
        entity = null;

        if (DataProvider.nowGameData.stockIsFull)
        {
            return false;
        }
        else if (GetCost(type) < DataProvider.nowGameData.resourceTable[ItemID.resource])
        {
            return false;
        }
        else if (type == BotType.MOTHER)
        {
            return false;
        }
        else
        {
            DataProvider.nowGameData.resourceTable[ItemID.resource] -= GetCost(type);
            entity = ArmBotData.CreateInstance(type);

            DataProvider.nowGameData.AddStock(entity);
            return true;
        }
    }


    public static bool AddStock(ArmBotData.Entity entity)
    {
        return DataProvider.nowGameData.AddStock(entity);
    }
    public static void RequestSummary(string botId, Vector2Int startCoords, Vector2 direction)
    {
        var entity = DataProvider.nowGameData.stocks.First(x => x.id == botId);
        entity.facingDirection = direction;
        var data = new SessionData(entity, startCoords);
        var arg = new SessionEventArg(SessionState.requestSummary, data);

        EventManager.instance.Notice(EventName.SessionEvent, arg);
    }

    public static bool RequestSession(SessionData data)
    {
        var arg = new SessionEventArg(SessionState.requestSession, data);
        EventManager.instance.Notice(EventName.SessionEvent, arg);
        return true;
    }
}