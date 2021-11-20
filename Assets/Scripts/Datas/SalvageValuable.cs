using UnityEngine;
using System;
using Sirenix.OdinInspector;

//データのやり取りや保存に使うSalvageData
public class SalvageValuable<T> : ScriptableObject, ISalvageData
{
    public event Action<T> onValueChanged;
    [ShowInInspector, ReadOnly]
    public T value
    {
        get
        {
            return _value;
        }
        set
        {
            if (onValueChanged != null)
            {
                onValueChanged(value);
            }
            _value = value;
        }
    }

    [SerializeField] T _value;
}