using System.Collections.Generic;
using System.Linq;
using System;

public class DataIndexer : ISalvageData
{
    public List<ISalvageData> entity { get { return datas; } }
    List<ISalvageData> datas;
    public bool readOnly { get; private set; }
    public DataIndexer()
    {
        datas = new List<ISalvageData>();
    }

    public DataIndexer(List<ISalvageData> datas, bool readOnly = false)
    {
        this.datas = datas;
        this.readOnly = readOnly;
    }
    public ISalvageData this[int i]
    {
        get { return datas[i]; }
        set { datas[i] = value; }
    }

    public void Add(ISalvageData element)
    {
        if (!readOnly)
        {
            datas.Add(element);
        }
        else
        {
            UnityEngine.Debug.Log("this Indexer is Read Only!");
        }
    }

    public T GetData<T>(int index)
    where T : ISalvageData
    {
        try
        {
            return (T)datas[index];
        }
        catch(IndexOutOfRangeException)
        {
            return default;
        }
    }

    public T GetData<T>(Predicate<T> predicator)
    where T:ISalvageData
    {
        return (T)datas.Find(x => predicator((T)x));
    }

    public bool Remove(ISalvageData element)
    {
        return datas.Remove(element);
    }

    public List<ISalvageData>.Enumerator GetEnumerator()
    {
        return datas.GetEnumerator();
    }

}