using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//島をならべしもの。
public class IslandRenderer : MonoBehaviour, IUIRenderer
{
    [SerializeField] InstantPool<SectorStepObject> stepPool;
    [SerializeField] SectorStepObject islandPref;
    [SerializeField] int initNum = 50;
    //1グリッド = ?
    [SerializeField] float coordinateGap{get{return StepGenerationConfig.instance.gridToCanvasrate;}}
    [SerializeField] Transform islandOrigin;
    List<SectorStepObject> stepObjects;

    [Sirenix.OdinInspector.Button]
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

        stepObjects = new List<SectorStepObject>();
        var maxMiasma = StepGenerationConfig.instance.maxMiasma;

        foreach (var step in map.mapData)
        {
            var obj = stepPool.GetObj();
            var localCoords = step.Key - originCoords;
            obj.transform.localPosition =  localCoords*coordinateGap;

            obj.UpdateData(step.Value);
            
            var miasma = map.miasmaMap[step.Key.x,step.Key.y];
            var colorNorm = miasma/maxMiasma;
            obj.islandImage.color = new Color(colorNorm,1-colorNorm,1-colorNorm,1);
            step.Value.name = obj.name;
            stepObjects.Add(obj);
        }

        for(int i = 0;i < stepObjects.Count;i++)
        {
           var renderTask = stepObjects[i].Show();
           yield return new WaitUntil(()=>renderTask.compleated);
        }

        #if DEBUG
        var logString = new StringBuilder();
        
        var count = 0;
        foreach(var step in map.mapData)
        {
            var obj = stepObjects[count];
            logString.Append("----Name:");
            logString.AppendLine(step.Value.name);

            logString.Append("localPosition");
            logString.AppendLine(obj.transform.localPosition.ToString());

            logString.Append("Position");
            logString.AppendLine(obj.transform.position.ToString());
            
            logString.Append("Coordinate:");
            logString.AppendLine(step.Key.ToString());

            logString.Append("Resource:");
            logString.AppendLine(step.Value.resourceLv.ToString());

            logString.Append("Miasma:");
            logString.AppendLine(map.miasmaMap[step.Key.x,step.Key.y].ToString());
            logString.Append("\n\n\n");
        }

        Utility.LogWriter.Log(logString.ToString(),"IslandLog",false);
        #endif


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