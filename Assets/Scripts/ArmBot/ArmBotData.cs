using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    public abstract BotType type { get; }

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
    public abstract class Entity : ISalvageData
    {
        //変更されてはたまったものじゃないので公開しない
        //session中、jsonで保存するためにSerializeする
        [SerializeField] BotStatusList defaultStatus;
        [SerializeField] EquipmentHolder equipment;
        [SerializeField] InventoryData inventory;
        public StatusType[] statusTypes { get { return nowStatus.types; } }
        public string id { get; }
        public BotType type { get; }
        public Vector2 facingDirection { get; set; }
        public List<EntityMod> mods = new List<EntityMod>();
        public Entity realBody{get;private set;}
        public bool isGhost{get{return realBody!=null;}}


        protected BotStatusList nowStatus;
        public int hp
        {
            get { return nowStatus.GetValue(StatusType.hp).Value; }
            set { nowStatus.SetValue(StatusType.hp, value); }
        }

        public float normalizedHp
        {
            get { return nowStatus.GetValue(StatusType.hp).Value / defaultStatus.GetValue(StatusType.hp).Value; }
        }

        public virtual Entity GetGhost()
        {
            if(this.isGhost){Debug.LogError("I am ghost");}

            var copy = this.MemberwiseClone() as Entity;
            copy.nowStatus = this.nowStatus.CopySelf();
            copy.mods = this.mods.GetRange(0,mods.Count);
            copy.realBody = this;
            return copy;
        }

        public Entity(ArmBotData data, BotType type)
        {
            defaultStatus = data.status;
            nowStatus = data.status.CopySelf();
            equipment = data._equipment;
            inventory = new InventoryData();
            this.type = type;
            this.id = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public virtual bool OnUpdate(SectorMap map, Vector2 coordinate)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                if (mods[i].exTiming == StepActionType.interact)
                {
                    mods[i].Execute(this);
                }
            }

            var delta = -(int)MapUtility.GetMiasmaDamage(coordinate);
            hp += delta;
            
            var barg = new BotEventArg(this, BotActionType.damaged, StatusType.hp, delta);
            EventManager.instance.Notice(EventName.BotEvent, barg);

            return true;
        }


        /// <summary>
        /// 継承先で呼んでください
        /// 私は知りません。
        /// </summary>
        protected virtual void Move()
        {
            var delta = facingDirection.normalized * GetStatus(StatusType.speed) * SessionConfig.instance.speedMultiplier;

            var arg = new TravelExArg(this, delta);
            EventManager.instance.Notice<ExploreArg>(EventName.SystemExploreEvent, arg);

        }


        //終了条件はbotによって違う
        public abstract bool CheckIfEnd();

        public int GetStatus(StatusType type)
        {
            return nowStatus.GetValue(type).Value;
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
            EventManager.instance.Notice<ExploreArg>(EventName.SystemExploreEvent, arg);

            return inventory.items.Count;
        }

        public bool ModStatus(StatusType statusType, Func<int, int> modifier)
        {
            var max = InochiConfig.GetMaxValue(type, statusType);
            var value = nowStatus.GetValue(statusType).Value;

            value = modifier(value);
            if (max != null && value > max)
            {
                return false;
            }

            nowStatus.SetValue(statusType, value);
            return true;
        }
    }

    protected List<ArmBotData> datas = new List<ArmBotData>();

    protected abstract Entity CreateInstance();


    public static Entity CreateInstance(BotType type)
    {
        for (int i = 0; i < variantInstances.Count; i++)
        {
            if (variantInstances[i].type == type)
            {
                return variantInstances[i].CreateInstance();
            }
        }

        Debug.LogError("Bot of type:" + type + " not found");
        return null;
    }
}