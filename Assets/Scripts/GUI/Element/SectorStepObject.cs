using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(IUIRenderer))]
public class SectorStepObject : MonoBehaviour,IUIElement
{
    new public IUIRenderer renderer{get;private set;}

    //Debugのみのやつ
    [SerializeField] TMPro.TMP_Text sourceIndicator;
    [SerializeField] UnityEngine.UI.Image image;
    public Image islandImage{get{return image;}}

    void Awake()
    {
        renderer = GetComponent<IUIRenderer>();
    }

    public ITask UpdateData(Island data)
    {
        sourceIndicator.text = data.resourceLv.ToString();
        return SmallTask.nullTask;
    }
}