using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;

//Dataをfromからtoにコピー
//現在コピー先はValuable<SalD>のみを想定している
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class DataImportButton : MonoBehaviour
{
    [SerializeField] AssetLabelReference fromRef;
    [SerializeField] AssetLabelReference toRef;
    protected DataIndexer fromData;
    protected DataIndexer toData;

    void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(() => OnClick());
    }

    protected virtual void OnClick()
    {
        var to = toData[0] as SalvageValuable<ISalvageData>;

        to.value = fromData;
    }

    IEnumerator LoadAsync()
    {
        var fromTask = DataManager.LoadDatasAsync(fromRef);
        var toTask = DataManager.LoadDatasAsync(toRef);

        yield return new WaitUntil(() => fromTask.ready);
        yield return new WaitUntil(() => toTask.ready);

        fromData = fromTask.result;
        toData = toTask.result;
    }

    void OnEnable()
    {
        StartCoroutine(LoadAsync());
    }

    void OnDisable()
    {
        DataManager.ReleaseDatas(fromData);
        DataManager.ReleaseDatas(toData);
    }
}