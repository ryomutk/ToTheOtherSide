using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//島をならべしもの。
//
public class IslandUIController : UIPanel
{
    public override PanelName name { get { return PanelName.IslandPanel; } }
    [SerializeField] InstantPool<SectorStepObject> stepPool;
    [SerializeField] SectorStepObject islandPref;
    [SerializeField] int initNum = 50;
    [SerializeField] RectTransform islandOrigin;
    List<SectorStepObject> stepTable = new List<SectorStepObject>();



    //Logをとるためのビルダ
    StringBuilder logString = new StringBuilder();


    protected override ITask Show()
    {
        var task = new SmallTask();


        StartCoroutine(DrawMap(task));

        return task;
    }

    protected override ITask Hide()
    {

        var task = new SmallTask();
        StartCoroutine(HideMap(task));

        return task;
    }

    IEnumerator DrawMap(SmallTask task)
    { 
        var map = DataProvider.nowGameData.map;
        var mapTask = new SmallTask();

        var baseDraw = base.Show();

        yield return new WaitUntil(()=>baseDraw.compleated);

        
        stepPool = new InstantPool<SectorStepObject>(islandOrigin);
        stepPool.CreatePool(islandPref, initNum, false);

        stepTable.Clear();
        var maxMiasma = StepGenerationConfig.instance.maxMiasma;

        foreach (var step in map.mapData)
        {
            var obj = stepPool.GetObj();

            obj = StepGenerationConfig.instance.InitializeStepObj(step.Value, step.Key, obj);

            //ちょっとした色付け
            var miasma = map.miasmaMap[step.Key.x, step.Key.y];
            var colorNorm = miasma / maxMiasma;
            obj.islandImage.color = new Color(colorNorm, 1 - colorNorm, 1 - colorNorm, 1);

            stepTable.Add(obj);
        }

        for (int i = 0; i < stepTable.Count; i++)
        {
            var obj = stepTable[i];
            var showTask = obj.renderer.Draw();
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
            var hideTask = step.renderer.Hide();
            yield return new WaitUntil(() => hideTask.compleated);
        }
        UnloadData();

        task.compleated = true;
    }

    void UnloadData()
    {
        stepTable = null;
        stepPool = null;
    }

}