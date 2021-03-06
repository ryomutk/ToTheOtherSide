using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

//DataLabelをなるべく使わないようにする目的で作られた。
public class DataProvider : Singleton<DataProvider>, IEventListener<SystemEventArg>
{
    SalvageValuable<ISalvageData> gameData;
    [SerializeField] AssetLabelReference l_gameDataLabel;

    [ShowInInspector, ReadOnly]
    public static GameSessionData nowGameData
    {
        get
        {
            if (instance!=null&& instance.gameData != null)
            {

                return instance.gameData.value as GameSessionData;
            }
            return null;
        }
    }

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
        gameData = loadTask.result as SalvageValuable<ISalvageData>;


        gameData.value = new GameSessionData();

        InitMap();
        nowGameData.currentMOTHERCoordinate = StepGenerationConfig.instance.originCoords;

        nowGameData.resourceTable[ItemID.resource] = 0;
        nowGameData.resourceTable[ItemID.cristal] = 0;

        nowGameData.MOTHER = ArmBotData.CreateInstance(BotType.MOTHER);

        SessionUtility.TryCreateNewInochi(BotType.searcher);

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