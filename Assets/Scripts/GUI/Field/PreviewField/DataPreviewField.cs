using UnityEngine;

//渡されたデータのプレビューを表示するField
//DataLayoutFieldがForｍ全体で、これは要素..みたいな感じ
public abstract class DataPreviewField:MonoBehaviour
{
    public abstract void LoadData(ISalvageData data);   
}