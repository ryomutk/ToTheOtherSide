using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(IUIRenderer),typeof(Button))]
public class  StockInochiButton:MonoBehaviour,IUIElement
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] Slider hpGauge;
    //持っているStatusの種類も数も違うので親への参照を登録してください
    //あとはこちらで管理します
    [SerializeField] Transform statusTextArea;
    Button entityButton;
    TMP_Text[] statusTexts;
    new public IUIRenderer renderer{get;private set;}
    string botId;

    void Start()
    {
        entityButton = GetComponent<Button>();
        renderer = GetComponent<IUIRenderer>();
    }

    public void Initialize(ArmBotData.Entity entity)
    {
        botId = entity.id;
        nameText.text = entity.type.ToString();
        hpText.text = entity.hp.ToString() + "/" + ((int)(entity.hp/entity.normalizedHp)).ToString();

        statusTexts = statusTextArea.GetComponentsInChildren<TMP_Text>();

        for(int i =0;i < entity.statusTypes.Length;i++)
        {
            var type = entity.statusTypes[i];
            if(type == StatusType.hp){continue;}
            statusTexts[i].text = type.ToString() + ":"+entity.GetStatus(type);
        }

        hpGauge.normalizedValue = entity.normalizedHp;
    }

}