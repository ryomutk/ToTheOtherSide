using UnityEngine;
using System.Collections;

//島をならべしもの。
//常駐にするつもりなので消したりするアレはない
public class IslandScreen : MonoBehaviour, IInformationField
{
    [SerializeField] InstantPool<SectorStepObject> sectorStep;
    [SerializeField] SectorStepObject islandPref;
    [SerializeField] int initNum = 50;


    public ITask LoadDataAsync()
    {
        var map = DataProvider.nowGameData.map;

    }

    IEnumerator DrawMap(SmallTask task, SectorMap map)
    {
        sectorStep = new InstantPool<SectorStepObject>(transform);
        sectorStep.CreatePool(islandPref,initNum,true);

        

        task.compleated = true;
    }

    public bool UnloadData()
    {
        sectorStep = null;
    }

}