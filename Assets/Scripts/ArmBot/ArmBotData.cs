using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu]
public class ArmBotData : ScriptableObject, ISalvageData
{
    //内部情報いじらせると出口入り口できぬゆえ。
    /*
    public BotStatusList status { get { return _status; } }
    public EquipmentHolder equipment { get { return _equipment; } }
    */

    //public InventoryData inventory { get { return _inventory; } }

    [SerializeField] BotStatusList _status;
    [SerializeField] EquipmentHolder _equipment;
    [SerializeField] SalvageEvent<ExploreArg> exploreEvent;

    //これはセッション中のアイテムがストックされる場所なので、いらない。
    //最初に持たせるものはすべてEquipmentHodlerへ。
    //名前変えたほうがいいかも
    //[SerializeField] InventoryData _inventory;

    public bool Equip(EquipmentItemData equip)
    {
        //勝手に外したくない場合ここでどうにかする
        Disarm(equip.targetPart);

        equip.OnEquip(this);
        _equipment.Set(equip);
        return true;
    }

    public bool Disarm(BodyParts part)
    {
        var target = _equipment.GetEquipment(part);

        if (target != null)
        {
            target.OnDisarm(this);
            _equipment.Remove(target.targetPart);

            return true;
        }

        return false;
    }


    //セッションで使うInstanceを取得
    [Serializable]
    public class Entity
    {
        //変更されてはたまったものじゃないので公開しない
        //session中、jsonで保存するためにSerializeする
        [SerializeField] BotStatusList defaultStatus;
        [SerializeField] EquipmentHolder equipment;
        [SerializeField] InventoryData inventory;
        Action<ExploreArg> noticer;

        BotStatusList nowStatus;
        public int genki
        {
            get { return nowStatus.GetValue(StatusType.genki); }
            set { nowStatus.SetValue(StatusType.genki, value); }
        }
        public int searchP { get { return nowStatus.GetValue(StatusType.search); } }


        public Entity(ArmBotData data)
        {
            defaultStatus = data._status;
            nowStatus = data._status.CopySelf();
            equipment = data._equipment;
            inventory = new InventoryData();
            noticer = (x) => data.exploreEvent.Notice(x);
        }

        //まだ動ける？
        public bool CheckMovility()
        {
            //ここは変わるかも
            return nowStatus.GetValue(StatusType.genki) > 0 && nowStatus.GetValue(StatusType.hp) > 0;
        }

        public bool HasEquip(ItemID id)
        {
            if(id == 0)
            {
                return true;
            }
            
            return equipment.Has(id);
        }

        public int GiveItem(ItemID item)
        {
            inventory.Add(item);

            var arg = new ItemExArg(item, ItemActionType.get);
            noticer(arg);

            return inventory.items.Count;
        }
    }

    public Entity CreateInstance()
    {
        return new Entity(this);
    }
}