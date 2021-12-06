using UnityEngine;
using UnityEngine.UI;

public class SessionSignalEmitter:MonoBehaviour
{
    [SerializeField]BotType type;

    void Start()
    {
        var button =  GetComponent<Button>();
        if(button!=null)
        {button.onClick.AddListener(()=>Emit());}
    }

    public void Emit()
    {
        SessionUtility.TryCreateNewInochi(type);
    }
}