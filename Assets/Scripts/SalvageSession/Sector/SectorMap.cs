using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SectorMap
{
    Dictionary<Vector2Int, SectorStep> _mapData;
    public Dictionary<Vector2Int, SectorStep> mapData { get { return _mapData; } }

    public bool SetIsland()
    {
        
    }


    //座標を探る。Rangeを入れると、その範囲内にあるシマの中で一番座標に近いものを返す
    public SectorStep CheckCoordinate(Vector2Int coordinate, float range = 0)
    {
        float sqrDistance;
        foreach (var stepCoordPair in _mapData)
        {
            sqrDistance = Vector2.Distance(stepCoordPair.Key, coordinate);
            if (sqrDistance < range)
            {
                return stepCoordPair.Value;
            }
        }

        return null;
    }

    /// <summary>
    /// 範囲(円)内にあるすべてのシマを返す
    /// </summary>
    /// <param name="coordinate">中心の座標</param>
    /// <param name="range">範囲円の半径</param>
    /// <param name="result">結果を入れるやつ</param>
    /// <param name="refreshResult">結果を都度Clearするか</param>
    /// <returns></returns>
    public bool CheckRange(Vector2Int coordinate, float range, ref List<SectorStep> result, bool refreshResult = false)
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