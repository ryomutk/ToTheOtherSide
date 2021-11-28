using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class SectorMap
{
    Dictionary<Vector2Int, SectorStep> _mapData = new Dictionary<Vector2Int, SectorStep>();
    public Dictionary<Vector2Int, SectorStep> mapData { get { return _mapData; } }
    public int[,] miasmaMap{get;private set;}

    public void SetMiasma(int[,] miasmaMap)
    {
        this.miasmaMap = miasmaMap;
    }

    public bool TryAddStep(int x,int y, SectorStep step)
    {
        if(!CheckCoordinate(x,y,step.radius))
        {
            var coordinate = new Vector2Int(x,y);
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
    public bool CheckCoordinate(int x,int y,float range = 0)
    {
        float sqrDistance;
        foreach (var stepCoordPair in _mapData)
        {
            var xdir = stepCoordPair.Key.x - x;
            var ydir = stepCoordPair.Key.y - y;
            sqrDistance = Mathf.Pow(xdir,2)+Mathf.Pow(ydir,2);

            if (sqrDistance < Mathf.Pow(range*2,2))
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
    public int TryFindRange(Vector2 coordinate, float range, ref List<SectorStep> result, bool refreshResult = false)
    {
        float sqrDistance;
        int foundCount = 0;
        foreach (var stepCoordPair in _mapData)
        {
            sqrDistance = Vector2.Distance(stepCoordPair.Key, coordinate);
            if (sqrDistance < Mathf.Pow(range*2, 2))
            {
                result.Add(stepCoordPair.Value);
                foundCount++;
            }
        }

        return foundCount;
    }
}