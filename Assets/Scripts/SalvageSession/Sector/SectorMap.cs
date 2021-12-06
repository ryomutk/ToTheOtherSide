using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SectorMap
{
    Dictionary<Vector2Int, Island> _mapData = new Dictionary<Vector2Int, Island>();
    public Dictionary<Vector2Int, Island> mapData { get { return _mapData; } }
    public int[,] miasmaMap { get; private set; }

    public Vector2 GetCoordinate(Island target)
    {
        var enumrator = _mapData.GetEnumerator();

        foreach (var data in mapData)
        {
            if (data.Value == target)
            {
                return data.Key - StepGenerationConfig.instance.originCoords;
            }
        }

        //何の脈絡もないTimezoneNotFoundException
        throw new System.TimeZoneNotFoundException();
    }

    public bool RemoveIsland(Island remove)
    {
        foreach (var step in mapData)
        {
            if (step.Value == remove)
            {
                mapData.Remove(step.Key);
                return true;
            }
        }

        return false;
    }

    public void SetMiasma(int[,] miasmaMap)
    {
        this.miasmaMap = miasmaMap;
    }

    public Island GetIsland(int id)
    {
        foreach (var data in mapData)
        {
            if (data.Value.id == id)
            {
                return data.Value;
            }
        }

        return null;
    }

    /// <summary>
    /// 可能なら島を加えたい人向け。
    /// </summary>
    /// <param name="x">座標</param>
    /// <param name="y">座標</param>
    /// <param name="step">加えるやつ</param>
    /// <param name="safeRange">不可侵領域</param>
    /// <returns></returns>
    public bool TryAddStep(int x, int y, Island step, float safeRange = 0)
    {
        if (!CheckCoordinate(x, y, step.radius + safeRange))
        {
            var coordinate = new Vector2Int(x, y);
            _mapData[coordinate] = step;

            return true;
        }
        return false;
    }


    /// <summary>
    /// 座標と範囲円の半径でその中に島があるかを確認する
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool CheckCoordinate(int x, int y, float range = 0)
    {
        float sqrDistance;
        foreach (var stepCoordPair in _mapData)
        {
            var xdir = stepCoordPair.Key.x - x;
            var ydir = stepCoordPair.Key.y - y;
            sqrDistance = Mathf.Pow(xdir, 2) + Mathf.Pow(ydir, 2);

            if (sqrDistance < Mathf.Pow(range + stepCoordPair.Value.radius, 2))
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 範囲円の中にあるすべてのシマを取得する
    /// </summary>
    /// <param name="coordinate">中心の座標</param>
    /// <param name="range">範囲円の半径</param>
    /// <param name="result">結果を入れるやつ</param>
    /// <param name="refreshResult">結果を都度Clearするか</param>
    /// <returns>見つけた島の数</returns>
    public int TryFindRange(Vector2 coordinate, float range, ref List<Island> result, bool refreshResult = false)
    {
        if (refreshResult)
        {
            result.Clear();
        }

        float sqrDistance;
        int foundCount = 0;
        foreach (var stepCoordPair in _mapData)
        {
            sqrDistance = Vector2.Distance(stepCoordPair.Key, coordinate);
            if (sqrDistance < Mathf.Pow(range + stepCoordPair.Value.radius, 2))
            {
                result.Add(stepCoordPair.Value);
                foundCount++;
            }
        }

        return foundCount;
    }

    public int TryFindRange(Vector2 coordinate, float range, ref List<int> result, bool refreshResult = false)
    {
        if (refreshResult)
        {
            result.Clear();
        }

        float sqrDistance;
        int foundCount = 0;
        foreach (var stepCoordPair in _mapData)
        {
            sqrDistance = Vector2.Distance(stepCoordPair.Key, coordinate);
            if (sqrDistance < Mathf.Pow(range + stepCoordPair.Value.radius, 2))
            {
                result.Add(stepCoordPair.Value.id);
                foundCount++;
            }
        }

        return foundCount;
    }
}