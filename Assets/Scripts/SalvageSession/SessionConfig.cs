using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class SessionConfig : ScriptableObject
{
    public static SessionConfig instance;

    void OnValidate()
    {
        Debug.Log("initialized");
        instance = this;
    }

    [SerializeField] List<MultiplierData> _multipliers;
    //-n～nがaccesabilityに足される。
    [SerializeField] int _accRandomizer = 5;
    [SerializeField] int _interactFuel = 2;
    [SerializeField] float _durationPerStep = 30;
    [SerializeField] int searchRandomizer = 10;

    public float durationPerStep { get { return _durationPerStep; } }
    public int accRandomizer { get { return _accRandomizer; } }
    public int interactFuel { get { return _interactFuel; } }
    public float depthPerDurability = 0.1f;
    public float GetMultiplier(StepState state)
    {
        return _multipliers.Find(x => x.state == state).multiplier;
    }

    [System.Serializable]
    class MultiplierData
    {
        public StepState state;
        public float multiplier;
    }
}