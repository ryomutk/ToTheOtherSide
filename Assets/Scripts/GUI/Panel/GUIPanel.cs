using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections;

[DisallowMultipleComponent]
public abstract class GUIPanel : MonoBehaviour
{
    new public PanelName name { get { return _name; } }
    [SerializeField] PanelName _name;
    [SerializeField] bool showBackPanel = true;
    IInformationField field;

    [ShowInInspector, ReadOnly]
    protected bool isActive { get; set; }
    public virtual bool Show()
    {
        isActive = true;
        if (showBackPanel)
        {
            UIManager.instance.ShowPanel(PanelName.BackButtonPanel);
        }
        return true;
    }

    public virtual bool Hide()
    {
        isActive = false;

        if (field != null)
        {
            field.UnloadData();
        }

        return true;
    }

    protected virtual void Start()
    {
        isActive = false;
        UIManager.instance.RegisterPanel(this);
        field = GetComponent<IInformationField>();
    }

    protected virtual IEnumerator ShowField()
    {
        if (field != null)
        {
            var task = field.LoadDataAsync();
            yield return new WaitUntil(() => task.ready);
        }
    }
}