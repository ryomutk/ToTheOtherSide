using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Book")]
public class BookItemData : ResourceData
{
    //BookItemはリソースなのか？
    public override ItemType type { get { return ItemType.resource; } }
    public override ItemAttribute attribute { get { return _attribute; } }
    public string title { get { return _title; } }
    public string content { get { return _content; } }

    [SerializeField] string _title;
    [SerializeField, Multiline] string _content;

    void OnValidate()
    {
        //必ずisBookは選ばれてるようにする
        if(!attribute.HasFlag(ItemAttribute.isBook))
        {
            _attribute += (int)ItemAttribute.isBook;
        }
    }
}