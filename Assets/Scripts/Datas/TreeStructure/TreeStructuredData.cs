using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;

//Base class to make Scriptable tree Structured data
public abstract class ScriptableTreeStructuredData<T> : ScriptableTreeStructureDataBase
where T : ScriptableTreeStructuredData<T>
{
    public T parent { get { return rawParent as T; } set { SetParent(value); } }

    public void AddChild(T child)
    {
        child.rawParent = this;

        _rawChildren.Add(child);
    }

    public T[] GetChildren()
    {
        T[] children = new T[_rawChildren.Count];

        for (int i = 0; i < _rawChildren.Count; i++)
        {
            children[i] = _rawChildren[i] as T;
        }

        return children;
    }

    //すべてのTree要素を連鎖的に集める仕組み
    //テスト中。
    public void GetAllOffsprings(ref List<T> output)
    {
        var children = GetChildren();

        output.Add(this as T);
        for (int i = 0; i < children.Length; i++)
        {
            children[i].GetAllOffsprings(ref output);
        }
    }

    public override bool SetParent(ScriptableTreeStructureDataBase parent)
    {
        if (parent == this)
        {
            Debug.Log("you can't be your father");
        }

        if (parent is T tmp)
        {
            SetParent(tmp);
            return true;
        }
        else if (parent == null)
        {
            SetParent(null);
            return true;
        }

        Debug.LogError(parent + " is not suitable type");
        return false;
    }

    public void SetParent(T parent)
    {
        if (this.parent != null)
        {
            this.parent._rawChildren.Remove(this as T);
        }

        rawParent = parent;

        if (parent != null)
        {
            parent._rawChildren.Add(this as T);
        }
    }

    public void RemoveChild(T child)
    {
        child.rawParent = null;
        _rawChildren.Remove(child);
    }

}


/// <summary>
/// base class of all TreeStructuredData
/// which don't require information of "T"
/// and doesn't have ways to modify data
/// </summary>
public abstract class ScriptableTreeStructureDataBase : ScriptableObject
{
    public ScriptableTreeStructureDataBase rawParent
    {
        get
        {
            if (_isRoot)
            {
                return null;
            }

            return _rawparent;
        }
        protected set
        {
            if (_isRoot)
            {
                Debug.LogWarning(name + " is root. Can't set parent");
            }
            else
            {
                _rawparent = value;
            }
        }
    }
    public ReadOnlyCollection<ScriptableTreeStructureDataBase> rawChildren { get { return _rawChildren.AsReadOnly(); } }
    [SerializeField, ReadOnly, PropertyOrder(1)] protected List<ScriptableTreeStructureDataBase> _rawChildren;

    [SerializeField, ReadOnly, PropertyOrder(1)] ScriptableTreeStructureDataBase _rawparent;

    [FoldoutGroup("Emergency"), Sirenix.OdinInspector.Button, PropertyOrder(1)]
    public abstract bool SetParent(ScriptableTreeStructureDataBase parent);

    public bool isRoot { get { return _isRoot; } set { _isRoot = value; } }
    [SerializeField, HideInInspector, PropertyOrder(1)] bool _isRoot = false;

    [FoldoutGroup("Emergency/Breaking"), Sirenix.OdinInspector.Button]
    void Clean()
    {
        _rawChildren = new List<ScriptableTreeStructureDataBase>();
        rawParent = null;
    }

    [FoldoutGroup("Emergency"), Button]
    bool GoUpward()
    {
        if (rawParent == null || rawParent.rawParent == null)
        {
            return false;
        }

        SetParent(rawParent.rawParent);
        return true;
    }
}