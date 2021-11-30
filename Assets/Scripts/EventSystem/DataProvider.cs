using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;

//DataLabelをなるべく使わないようにする目的で作られた。
public class DataProvider : Singleton<DataProvider>, IEventListener<SystemEventArg>
{
    SalvageValuable<ISalvageData> gameData;
    [SerializeField] AssetLabelReference l_gameDataLabel;

    public static GameSessionData nowGameData { get { return instance.gameData.value as GameSessionData; } }

    void Start()
    {
        EventManager.instance.Register(this, EventName.SystemEvent);
    }

    public ITask OnNotice(SystemEventArg arg)
    {
        if (arg.state == GameState.SystemInitialize)
        {
            var task = new SmallTask();
            StartCoroutine(DataLoadRoutine(task));

            return task;
        }

        return SmallTask.nullTask;
    }

    IEnumerator DataLoadRoutine(SmallTask task)
    {
        var loadTask = DataManager.LoadDataAsync(l_gameDataLabel);
        yield return new WaitUntil(() => loadTask.compleated);

        gameData.value = new GameSessionData();

        gameData = loadTask.result as SalvageValuable<ISalvageData>;

        var newGameDat = new GameSessionData();
        gameData.value = newGameDat;

        InitMap();
        nowGameData.currentMOTHERCoordinate = StepGenerationConfig.instance.originCoords;

        nowGameData.resourceTable[ItemID.resource] = 0;
        nowGameData.resourceTable[ItemID.cristal] = 0;

        

        task.compleated = true;
    }

    void InitMap()
    {
        var map = StepGenerationConfig.instance.GenerateMap();
        nowGameData.map = map;
    }

    void OnDisable()
    {
        DataManager.ReleaseData(gameData);
    }

}