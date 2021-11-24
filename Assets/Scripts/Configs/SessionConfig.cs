using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class SessionConfig : SingleScriptableObject<SessionConfig>
{
    [SerializeField] List<MultiplierData> _multipliers;

    [SerializeField] float _durationPerStep = 30;
    [SerializeField] SerializableDictionary<ExploreArg, float> durationTable;
    public float durationPerStep { get { return _durationPerStep; } }
    public float depthPerDurability = 0.1f;
    public float GetMultiplier(StepState state)
    {
        return _multipliers.Find(x => x.state == state).multiplier;
    }

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