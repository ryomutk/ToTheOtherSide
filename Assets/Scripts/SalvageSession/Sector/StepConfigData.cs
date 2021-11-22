using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu]
public class StepConfig : SingleScriptableObject<StepConfig>
{
    
    //全体のサイズ
    [SerializeField]Vector2 mapSize = new Vector2(50,50);
    
}
