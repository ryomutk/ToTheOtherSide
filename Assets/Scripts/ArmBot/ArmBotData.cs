using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[CreateAssetMenu]
public abstract class ArmBotData : ScriptableObject, ISalvageData
{
    //内部情報いじらせると出口入り口できぬゆえ。
    /*
    public BotStatusList status { get { return _status; } }
    public EquipmentHolder equipment { get { return _equipment; } }
    */

    //public InventoryData inventory { get { return _inventory; } }

    protected abstract BotStatusList status { get; }
    [SerializeField] public BotType type { get; }

    [SerializeField] EquipmentHolder _equipment;
    [SerializeField] protected SalvageEvent<ExploreArg> exploreEvent;

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
    public abstract class Entity
    {
        //変更されてはたまったものじゃないので公開しない
        //session中、jsonで保存するためにSerializeする
        [SerializeField] BotStatusList defaultStatus;
        [SerializeField] EquipmentHolder equipment;
        [SerializeField] InventoryData inventory;
        public BotType type { get; }
        Action<ExploreArg> noticer;
        Func<bool> endStatusCheck;

        BotStatusList nowStatus;
        public int hp
        {
            get { return nowStatus.GetValue(StatusType.hp); }
            set { nowStatus.SetValue(StatusType.hp, value); }
        }
        public int searchP { get { return nowStatus.GetValue(StatusType.search); } }


        public Entity(ArmBotData data)
        {
            defaultStatus = data.status;
            nowStatus = data.status.CopySelf();
            equipment = data._equipment;
            inventory = new InventoryData();
            noticer = (x) => data.exploreEvent.Notice(x);
        }

        //
        public abstract bool OnInteract(SectorStep step);
        //終了条件はbotによって違う
        protected abstract bool CheckIfEnd(Entity entity);



        public bool HasEquip(ItemID id)
        {
            if (id == 0)
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

    protected List<ArmBotData> datas = new List<ArmBotData>();

    protected abstract Entity CreateInstance();

    static public Entity CreateInstance(BotType type)
    {
        return 
    }
}