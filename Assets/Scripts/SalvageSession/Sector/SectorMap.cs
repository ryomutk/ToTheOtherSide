using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SectorMap
{
    Dictionary<Vector2Int, SectorStep> _mapData;
    public Dictionary<Vector2Int, SectorStep> mapData { get { return _mapData; } }

    public bool TryAddStep(Vector2Int coordinate, SectorStep step)
    {
        if(!CheckCoordinate(coordinate,step.radius))
        {
            _mapData[coordinate] = step;
        }
        return false;
    }


    /// <summary>
    /// 座標と範囲円の半径でその中に島があるかを確認する
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool CheckCoordinate(Vector2Int coordinate, float range = 0)
    {
        float sqrDistance;
        foreach (var stepCoordPair in _mapData)
        {
            sqrDistance = Vector2.Distance(stepCoordPair.Key, coordinate);
            if (sqrDistance < range)
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
    /// <returns></returns>
    public bool TryFindRange(Vector2Int coordinate, float range, ref List<SectorStep> result, bool refreshResult = false)
    {
        float sqrDistance;
        bool ifFound = false;
        foreach (var stepCoordPair in _mapData)
        {
            sqrDistance = Vector2.Distance(stepCoordPair.Key, coordinate);
            if (sqrDistance < Mathf.Pow(range, 2))
            {
                result.Add(stepCoordPair.Value);
                ifFound = true;
            }
        }

        return ifFound;
    }
}