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
    Dictionary<Vector2,SectorStepObject> stepTable;

    [Sirenix.OdinInspector.Button]
    public ITask Draw()
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

        stepTable = new Dictionary<Vector2, SectorStepObject>();
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
            stepTable[obj.transform.localPosition] = obj;
        }

        foreach(var obj in stepTable)
        {
            var showTask = obj.Value.Show();
            yield return new WaitUntil(()=>showTask.compleated);
        }

        #if DEBUG
        var logString = new StringBuilder();
        
        foreach(var step in map.mapData)
        {
            logString.Append("----Name:");
            logString.AppendLine(step.Value.name);
            
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
        foreach(var step in stepTable)
        {
            var hideTask = step.Value.Hide();
            yield return new WaitUntil(()=>hideTask.compleated);
        }

        task.compleated = true;
    }

    void UnloadData()
    {
        stepTable = null;
        stepPool = null;
    }

}