using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(IUIRenderer))]
public class SectorStepObject : MonoBehaviour, IUIRenderer
{
    IUIRenderer uiRenderer;

    //Debugのみのやつ
    [SerializeField] TMPro.TMP_Text sourceIndicator;
    [SerializeField] UnityEngine.UI.Image image;
    public Image islandImage{get{return image;}}

    void Awake()
    {
        uiRenderer = GetComponent<IUIRenderer>();
    }

    public ITask UpdateData(SectorStep data)
    {
        sourceIndicator.text = data.resourceLv.ToString();
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