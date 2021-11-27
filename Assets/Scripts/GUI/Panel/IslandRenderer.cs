using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//島をならべしもの。
public class IslandRenderer : MonoBehaviour, IUIRenderer
{
    [SerializeField] InstantPool<SectorStepObject> stepPool;
    [SerializeField] SectorStepObject islandPref;
    [SerializeField] int initNum = 50;
    //1グリッド = ?
    [SerializeField] float coordinateGap = 0.1f;
    [SerializeField] Transform islandOrigin;
    List<SectorStepObject> stepObjects;


    public ITask Show()
    {
        var map = DataProvider.nowGameData.map;
        var task = new SmallTask();
        StartCoroutine(DrawMap(task,map));

        return task;
    }

    public ITask Hide()
    {
        var task = new SmallTask();
        StartCoroutine(HideMap(task));

        return task;
    }

    IEnumerator DrawMap(SmallTask task, SectorMap map)
    {
        stepPool = new InstantPool<SectorStepObject>(islandOrigin);
        stepPool.CreatePool(islandPref, initNum, false);

        var originCoords = StepGenerationConfig.instance.originCoords;
        

        foreach (var step in map.mapData)
        {
            var obj = stepPool.GetObj();
            var localCoords = step.Key - originCoords;
            obj.transform.localPosition =  (Vector2)islandOrigin.localPosition  + localCoords*coordinateGap;
        }

        for(int i = 0;i < stepObjects.Count;i++)
        {
           var renderTask = stepObjects[i].Show();
           yield return new WaitUntil(()=>renderTask.compleated);
        }


        task.compleated = true;
    }

    IEnumerator HideMap(SmallTask task)
    {
        foreach(var step in stepObjects)
        {
            var hideTask = step.Hide();
            yield return new WaitUntil(()=>hideTask.compleated);
        }

        task.compleated = true;
    }

    void UnloadData()
    {
        stepObjects = null;
        stepPool = null;
    }

}