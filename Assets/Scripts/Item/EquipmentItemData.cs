using UnityEngine;

public class EquipmentItemData : ItemData
{
    [SerializeField] BodyParts _targetPart;

    //装備したときなんかしたいならこいつを登録。
    //大体は持ってるだけのフラグの権化的な効果だけにしたい。
    //nullなら何もしないのでなくても心配しなくていい
    [SerializeField] EquipmentBehaviour behaviour;
    public BodyParts targetPart { get { return _targetPart; } }
    public override ItemType type { get { return ItemType.equipment; } }

    //この辺はArmBotData使ってそのままDiscに上書きしちゃう
    public bool OnEquip(ArmBotData bot)
    {
        behaviour.OnEquip(bot);
        return true;
    }

    public bool OnDisarm(ArmBotData bot)
    {
        behaviour.OnRemove(bot);
        return true;
    }
}