using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Configs/Session")]
public class SessionConfig : SingleScriptableObject<SessionConfig>
{
//    [SerializeField,HideInInspector] SerializableDictionary<ExploreArg, float> durationTable;
//とりあえず手打ちしときますねー

    [SerializeField] float _speedMultiplier = 1;

    //スピードに対して、一つのステップで動く距離
    public float speedMultiplier{get{return _speedMultiplier;}}
    //最も短い時間の単位
    [SerializeField] float tickDuration = 0.5f;

    [ShowInInspector] public float sourceSieldMultiplier{get{return _sourceSheldMultiplier;} private set{_sourceSheldMultiplier = value;}}
    [SerializeField,HideInInspector]float _sourceSheldMultiplier = 0.1f;

    public float GetDuration(ExploreArg exploreArg)
    {
        //今はとりあえず動くのだけ実装
        if(exploreArg.type == ExploreObjType.Travel)
        {
            return tickDuration;
        }

        return 0;
    }

    [System.Serializable]
    class MultiplierData
    {
        public StepState state;
        public float multiplier;
    }
}