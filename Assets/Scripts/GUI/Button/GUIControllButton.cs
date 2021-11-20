using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[RequireComponent((typeof(Button)))]
public class GUIControllButton : MonoBehaviour
{
    public Button baseButton { get; private set; }
    public DataPreviewField iconField { get; private set; }
    [SerializeField] GUIControllAction[] actions;

    [System.Serializable]
    class GUIControllAction
    {
        [SerializeField] public PanelName targetPanel;
        [SerializeField] public PanelAction actionType;
        [SerializeField, ShowIf("actionType", PanelAction.show)] public ShowType method = ShowType.overrap;
    }

    public virtual void OnClick()
    {
        foreach (var action in actions)
        {
            if (action.actionType == PanelAction.hide)
            {
                UIManager.instance.HidePanel(action.targetPanel);
            }
            else
            {
                UIManager.instance.ShowPanel(action.targetPanel, action.method);
            }
        }
    }

    void Awake()
    {
        baseButton = GetComponent<Button>();
        baseButton.onClick.AddListener(OnClick);
    }

    protected void Start()
    {
        iconField = GetComponent<DataPreviewField>();
    }


}


public enum PanelAction
{
    show,
    hide
}