using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu]
public class SectorStepData : ScriptableTreeStructuredData<SectorStepData>, ISalvageData
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

    public int iD { get { return _id; } }
    [SerializeField] int _id;

    [ShowInInspector, ReadOnly]
    public int alignSector
    {
        get
        {
            if (!isRoot)
            {
                if(parent == null)
                {
                    return _alignSector;
                }
                return parent.alignSector;
            }
            else
            {
                return _alignSector;
            }
        }
    }

    public Sprite thumbNail { get { return _thumbnail; } }

    //Rootだけのためのメンバー
    //それ以外はフィールドのほうを参照すること
    [HideInInspector] public int _alignSector = 0;

    public SectorStep.StepBehaviourBase behaviour
    {
        get { return _behaviour; }
    }
    public EnvironmentAttribute defaultAttribute { get { return _attribute; } }
    public string detail { get { return _detail; } }
    public int rawDurability { get { return _durability; } }
    [SerializeField, HideInInspector] string _name;
    [SerializeField, HorizontalGroup("Split", 55, LabelWidth = 100)]
    [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
    Sprite _thumbnail;
    [SerializeField, VerticalGroup("Split/mid")] int _durability;
    [SerializeField, HideLabel, VerticalGroup("Split/mid"), EnumToggleButtons] EnvironmentAttribute _attribute;
    [SerializeField, VerticalGroup("Split/mid"), Multiline] string _detail;
    [SerializeField, TabGroup("Logs")] List<StepLogData> logDatas;
    //StepBehaviourの受けですべての継承クラスがシリアライズできるようにするすべを模索宇
    [SerializeField,TabGroup("Behaviour"),HideLabel] StepBehaviourPrototype _behaviour;
}