using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//島をならべしもの。
//
public class IslandUIController : MonoBehaviour
{
    [SerializeField] InstantPool<SectorStepObject> stepPool;
    [SerializeField] SectorStepObject islandPref;
    [SerializeField] int initNum = 50;
    [SerializeField] RectTransform islandOrigin;
    List<SectorStepObject> stepTable = new List<SectorStepObject>();
    bool showing = false;



    //Logをとるためのビルダ
    StringBuilder logString = new StringBuilder();


    [Sirenix.OdinInspector.Button]
    public ITask Draw()
    {
        if (!showing)
        {
            showing = true;

            var map = DataProvider.nowGameData.map;
            var task = new SmallTask();
            StartCoroutine(DrawMap(task, map));

            return task;
        }

        return SmallTask.nullTask;
    }

    public ITask Hide()
    {
        if (showing)
        {
            showing = false;

            var task = new SmallTask();
            StartCoroutine(HideMap(task));

            return task;
        }

        return SmallTask.nullTask;
    }

    IEnumerator DrawMap(SmallTask task, SectorMap map)
    {
        stepPool = new InstantPool<SectorStepObject>(islandOrigin);
        stepPool.CreatePool(islandPref, initNum, false);

        stepTable.Clear();
        var maxMiasma = StepGenerationConfig.instance.maxMiasma;

        foreach (var step in map.mapData)
        {
            var obj = stepPool.GetObj();

            obj = StepGenerationConfig.instance.InitializeStepObj(step.Value,step.Key,obj);

            //ちょっとした色付け
            var miasma = map.miasmaMap[step.Key.x, step.Key.y];
            var colorNorm = miasma / maxMiasma;
            obj.islandImage.color = new Color(colorNorm, 1 - colorNorm, 1 - colorNorm, 1);
            
            stepTable.Add(obj);
        }

        for (int i = 0; i < stepTable.Count; i++)
        {
            var obj = stepTable[i];
            var showTask = obj.Show();
            yield return new WaitUntil(() => showTask.compleated);
        }

#if DEBUG
        logString.Clear();

        int count = 0;
        foreach (var step in map.mapData)
        {
            var obj = stepTable[count];

            logString.Append("----Name:");
            logString.AppendLine(step.Value.name);

            logString.Append("Coordinate:");
            logString.AppendLine(step.Key.ToString());

            logString.Append("LocalPosition");
            logString.AppendLine(obj.transform.localPosition.ToString());

            logString.Append("Position");
            logString.AppendLine(obj.transform.position.ToString());

            logString.Append("Resource:");
            logString.AppendLine(step.Value.resourceLv.ToString());

            logString.Append("Miasma:");
            logString.AppendLine(map.miasmaMap[step.Key.x, step.Key.y].ToString());
            logString.Append("\n\n\n");

            count++;
        }

        Utility.LogWriter.Log(logString.ToString(), "IslandLog", false);
#endif


        task.compleated = true;
    }

    IEnumerator HideMap(SmallTask task)
    {
        foreach (var step in stepTable)
        {
            var hideTask = step.Hide();
            yield return new WaitUntil(() => hideTask.compleated);
        }

        task.compleated = true;
    }

    void UnloadData()
    {
        stepTable = null;
        stepPool = null;
    }

}