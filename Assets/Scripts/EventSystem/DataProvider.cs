using UnityEngine.AddressableAssets;
using UnityEngine;

//DataLabelをなるべく使わないようにする目的で作られた。
public class DataProvider:Singleton<DataProvider>,IEventListener<SystemEventArg>
{
    SalvageValuable<ISalvageData> gameData;
    [SerializeField] AssetLabelReference l_gameDataLabel;

    public static GameSessionData nowGameData{get{return instance.gameData.value as GameSessionData;}}

    void Start()
    {
        
    }

    public ITask OnNotice(SystemEventArg arg)
    {
        if(arg.state == GameState.SystemInitialize)
        {
            var map = StepGenerationConfig.instance.GenerateMap();
            var gameData = new GameSessionData();

            gameData.map = map;
        }

        return SmallTask.nullTask;
    }

    void OnDisable()
    {
        DataManager.ReleaseData(gameData);
    }

}