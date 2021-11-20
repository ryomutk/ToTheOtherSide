using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class QuestionArg : ChatArg
{
    [SerializeField] QandResponce[] responces; 
    string[] questions;

    void OnValidate()
    {
        questions = new string[responces.Length];
        
        for(int i = 0; i < responces.Length; i++)
        {
            questions[i] = responces[i].question;
        }
    }
    
    public TalkData GetResponce(int index)
    {
        return responces[index].responce;
    }

    public string[] GetQuestions()
    {        
        return questions;
    }
}

[System.Serializable]
public class QandResponce
{
    public string question{get{return _question;}}
    public TalkData responce{get{return _responce;}}
    [SerializeField]string _question;
    [SerializeField,InlineEditor]TalkData _responce;
}
