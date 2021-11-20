using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;

public class SEventListener : MonoBehaviour
{
    [SerializeField] AssetLabelReference eventLabel;
    SalvageEvent sEvent;


    void OnEnable()
    {
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        var task = DataManager.LoadDatasAsync(eventLabel);
        yield return new WaitUntil(()=>task.ready);

        sEvent = task.result.GetData<SalvageEvent>(0);
    }

    public void Notice()
    {
        sEvent.Notice();
    }
}