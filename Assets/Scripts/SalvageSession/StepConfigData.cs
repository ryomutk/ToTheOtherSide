using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu]
public class StepConfig : ScriptableObject
{
    [ShowInInspector, HideLabel, PropertyOrder(-1)]
    new public string name
    {
        get
        {
            if (_name == null)
            {
                return base.name;
            }

            return _name;
        }
        set { _name = value; }
    }

    [SerializeField] int _id;
    
    public int rawDurability { get { return _durability; } }
    [SerializeField, HideInInspector] string _name;
    [SerializeField, HorizontalGroup("Split", 55, LabelWidth = 100)]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
    Sprite _thumbnail;
    [SerializeField, VerticalGroup("Split/mid")] int _durability;
    [SerializeField, HideLabel, VerticalGroup("Split/mid"), EnumToggleButtons] EnvironmentAttribute _attribute;
    [SerializeField, VerticalGroup("Split/mid"), Multiline] string _detail;
    //StepBehaviourの受けですべての継承クラスがシリアライズできるようにするすべを模索宇
    [SerializeField, TabGroup("Behaviour"), HideLabel] StepBehaviourPrototype _behaviour;
}
