using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class SessionConfig : SingleScriptableObject<SessionConfig>
{
    [SerializeField] List<MultiplierData> _multipliers;

    [SerializeField] float _durationPerStep = 30;
    [SerializeField] SerializableDictionary<ExploreArg, float> durationTable;
    [SerializeField] float _speedMultiplier = 0.1f;

    //スピードに対して、一つのステップで動く距離
    public int speedMultiplier{get;}



    public float GetDuration(ExploreArg exploreArg)
    {
        if (durationTable.TryGetItem(exploreArg, out float duration))
        {
            return duration;
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