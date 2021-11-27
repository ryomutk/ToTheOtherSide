using UnityEngine;

[RequireComponent(typeof(IUIRenderer))]
public class SectorStepObject : MonoBehaviour, IUIRenderer
{
    IUIRenderer uiRenderer;

    void Start()
    {
        uiRenderer = GetComponent<IUIRenderer>();
    }

    public ITask UpdateData(SectorStep data)
    {
        return SmallTask.nullTask;
    }

    public ITask Show()
    {
        return uiRenderer.Show();
    }

    public ITask Hide()
    {
        return uiRenderer.Hide();
    }
}