using UnityEngine;

[CreateAssetMenu]
public class TalkData : ScriptableObject,ISalvageData
{   
    public bool hasQuestion{get{return _hasQuestion;}}
    public ChatArg[] chats { get; private set; }
    [SerializeField] bool _hasQuestion;
    [SerializeField] ChatArg[] _chats;

    //Chatsの後に読み込まれるQuestion
    //nullならそのまま進む
    [SerializeField] QuestionArg question = null;

    protected virtual void OnValidate()
    {
        chats = new ChatArg[_chats.Length + 1];
        if (question != null)
        {
            chats[_chats.Length] = question;
        }
    }
}