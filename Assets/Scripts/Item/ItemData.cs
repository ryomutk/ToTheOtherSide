using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ItemData : ScriptableObject, ISalvageData
{
    new public string name
    {
        get
        {
            if(_name == "")
            {
                return base.name;
            }
            return _name;
        }
    }
    [SerializeField] string _name = null;
    public ItemID id { get { return _id; } }
    public Sprite thumbNail { get { return _thumbnail; } }
    public abstract ItemType type { get; }
    public virtual ItemAttribute attribute { get { return _attribute; } }
    public bool isRelic { get { return _isRelic; } }
    public string discription { get { return _discription; } }
    [SerializeField, PreviewField(55, ObjectFieldAlignment.Left)] Sprite _thumbnail;
    [SerializeField] ItemID _id;
    [SerializeField] bool _isRelic;
    [SerializeField, Multiline] string _discription;
    [SerializeField, EnumToggleButtons] protected ItemAttribute _attribute;

}