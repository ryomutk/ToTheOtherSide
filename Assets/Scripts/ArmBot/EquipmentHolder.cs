using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


[Serializable]
public class EquipmentHolder : ISalvageData
{
    public EquipmentHolder()
    {
        equipments = new List<LabeledData<BodyParts, EquipmentItemData>>();
        var enumList = Enum.GetValues(typeof(BodyParts)) as BodyParts[];
        equipments = new List<LabeledData<BodyParts, EquipmentItemData>>();
        for (int i = 0; i < enumList.Length; i++)
        {
            var data = new LabeledData<BodyParts, EquipmentItemData>();
            data.type = enumList[i];
            equipments.Add(data);
        }
    }

    [SerializeField] List<LabeledData<BodyParts, EquipmentItemData>> equipments;

    public bool Set(EquipmentItemData equipment)
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i].type == equipment.targetPart)
            {
                var result = (equipments[i].value == null);

                if (result)
                {
                    equipments[i].value = equipment;
                }

                return result;
            }
        }

        throw new Exception("body part not implemented");
    }

    public bool Remove(BodyParts targetPart)
    {
        equipments.Find(x => x.type == targetPart);

        for (int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i].type == targetPart)
            {
                equipments[i].value = null;
                return true;
            }
        }

        throw new Exception("body part not implemented");
    }

    public EquipmentItemData GetEquipment(BodyParts targetPart)
    {
        var target = equipments.Find(x => x.type == targetPart);

        if(target == null)
        {
            throw new Exception("body part not implemented");
        }

        return target.value;
    }

    public bool Has(ItemID id)
    {
        return equipments.Any(x =>x.value != null && x.value.id == id);
    }
}