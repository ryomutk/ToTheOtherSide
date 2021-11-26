using UnityEngine.AddressableAssets;
using UnityEngine;


public class GameInitializer:MonoBehaviour,IEventListener<SystemEventArg>
{
    SalvageValuable<ISalvageData> gameData;
    [SerializeField] AssetLabelReference l_gameDataLabel;

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

}