using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu]
public abstract class ArmBotData : ScriptableObject, ISalvageData
{
    //内部情報いじらせると出口入り口できぬゆえ。
    /*
    public BotStatusList status { get { return _status; } }
    public EquipmentHolder equipment { get { return _equipment; } }
    */

    //public InventoryData inventory { get { return _inventory; } }

     protected abstract BotStatusList status{get;}
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

    protected abstract bool InteractAction(SectorStep step);
    //終了条件
    protected abstract bool EndStatus(Entity entity);


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
        Func<SectorStep, bool> interactAction;
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
            interactAction = data.InteractAction;
            endStatusCheck = () => data.EndStatus(this);
        }


        //まだ動ける？
        public bool CheckMovility()
        {
            return endStatusCheck();
        }



        public bool OnInteract(SectorStep step)
        {
            return interactAction(step);
        }

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

    protected Entity CreateInstance()
    {
        return new Entity(this);
    }
}