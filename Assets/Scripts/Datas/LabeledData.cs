using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class LabeledData<T,M>
where T:Enum
{
    public T type;

    [LabelText("$type")]
    public M value;
}

[Serializable]
public class  LabeledData<T>
where T:ISalvageData
{
    public string name;
    public T value;
}