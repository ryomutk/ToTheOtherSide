using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HideAllButton:MonoBehaviour
{
    Button baseButton;

    void Start()
    {
         baseButton = GetComponent<Button>();
         baseButton.onClick.AddListener(OnClick);
    }    
    
    void OnClick()
    {
        UIManager.instance.HideAllPanel();
    }
}