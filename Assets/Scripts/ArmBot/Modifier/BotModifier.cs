using UnityEngine;

public abstract class BotModifier:ScriptableObject
{
    public abstract void Apply(ArmBotData bot);
    public abstract void Remove(ArmBotData bot);
}