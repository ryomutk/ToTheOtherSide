using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[CreateAssetMenu]
public abstract class ArmBotData : SingleVariantScriptableObject<ArmBotData>, ISalvageData
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

        public Vector2 facingDirection { get; private set; }

        protected BotStatusList nowStatus;
        public int hp
        {
            get { return nowStatus.GetValue(StatusType.hp); }
            set { nowStatus.SetValue(StatusType.hp, value); }
        }

        public Entity(ArmBotData data, Vector2 faceDirection)
        {
            defaultStatus = data.status;
            nowStatus = data.status.CopySelf();
            equipment = data._equipment;
            inventory = new InventoryData();
        }

        public abstract bool OnInteract(SectorMap map,Vector2 coordinate);
        //終了条件はbotによって違う
        public abstract bool CheckIfEnd();

        public int GetStatus(StatusType type)
        {
            return nowStatus.GetValue(type);
        }


        //とりあえず外から方向を指定できるのは初期化の時だけにする
        /*
        public void SetFaceDirection(Vector2 direction)
        {
            facingDirection = direction;
        }
        */

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

            var arg = new ItemExArg(this, item, ItemActionType.get);
            EventManager.instance.Notice(EventName.SystemExploreEvent,arg);

            return inventory.items.Count;
        }
    }

    protected List<ArmBotData> datas = new List<ArmBotData>();

    public abstract Entity CreateInstance(Vector2 faceDirection);
}