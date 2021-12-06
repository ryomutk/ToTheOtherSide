using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//島をならべしもの。
public class IslandMapPanel : UIPanel
{
    public override PanelName name { get { return PanelName.IslandPanel; } }
    [SerializeField] InstantPool<SectorStepObject> stepPool;
    [SerializeField] SectorStepObject islandPref;
    [SerializeField] int initNum = 50;
    [SerializeField] RectTransform _viewPointTransform;

    //これ以上一気にZoomレベルが動いたらアニメーションを加える
    [SerializeField] float animateZoomDeltaValue = 1;
    public RectTransform viewpointOrigin { get { return _viewPointTransform; } }
    List<SectorStepObject> stepTable = new List<SectorStepObject>();

    //Logをとるためのビルダ
    StringBuilder logString = new StringBuilder();

    IEnumerator FocusRoutine(Vector2 viewPosition, SmallTask task)
    {
        viewpointOrigin.position = viewPosition * -1 * viewpointOrigin.localScale.x;
        yield return null;
        task.compleated = true;
    }

    IEnumerator ZoomRoutine(float zoomLevel, SmallTask task)
    {
        var last = viewpointOrigin.localScale.x;

        yield return StartCoroutine(HideMap());

        viewpointOrigin.position *= zoomLevel / last;
        viewpointOrigin.localScale = new Vector2(zoomLevel, zoomLevel);

        yield return StartCoroutine(DrawMap());

        task.compleated = true;
    }

    public ITask Zoom(float zoomLevel)
    {
        var task = new SmallTask();
        var last = viewpointOrigin.localScale.x;
        if (Mathf.Abs(zoomLevel - last) > animateZoomDeltaValue)
        {
            StartCoroutine(ZoomRoutine(zoomLevel, task));
        }
        else
        {
            viewpointOrigin.position *= zoomLevel / last;
            viewpointOrigin.localScale = new Vector2(zoomLevel, zoomLevel);
            task.compleated = true;
        }

        return task;
    }

    public ITask Focus(Vector2 coordinate)
    {
        var task = new SmallTask();
        var position = coordinate * StepGenerationConfig.instance.gridToCanvasrate;

        StartCoroutine(FocusRoutine(position, task));
        return task;
    }

    public ITask Focus(Island target)
    {
        var coordinate = DataProvider.nowGameData.map.GetCoordinate(target);
        return Focus(coordinate);
    }


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

    IEnumerator DrawMap(SmallTask task = null)
    {
        var map = DataProvider.nowGameData.map;
        var mapTask = new SmallTask();

        var baseDraw = base.Show();

        yield return new WaitUntil(() => baseDraw.compleated);


        stepPool = new InstantPool<SectorStepObject>(viewpointOrigin);
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


        if (task != null)
        {
            task.compleated = true;
        }
    }

    IEnumerator HideMap(SmallTask task = null)
    {
        foreach (var step in stepTable)
        {
            var hideTask = step.renderer.Hide();
            yield return new WaitUntil(() => hideTask.compleated);
        }

        if (task != null)
        {
            task.compleated = true;
        }
    }

    void UnloadData()
    {
        stepTable.Clear();
        stepPool = null;
    }

}