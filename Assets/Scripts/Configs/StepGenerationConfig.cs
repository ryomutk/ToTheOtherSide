using UnityEngine;

public class StepGenerationConfig : SingleScriptableObject<StepGenerationConfig>
{
    [SerializeField] float maxMiasma = 200;
    [SerializeField] Vector2Int mapSize = new Vector2Int(100, 100);
    [SerializeField] int islandSize = 1;
    //マスX,Yに島が生成される最大の確立
    [SerializeField] float islandPossibility = 0.1f;
    [SerializeField] float maxPossibilityDiscount = 0.03f;

    [SerializeField] bool randomizeGoal;

    //その場所のMiasmaにこれがかけられた値がその場所の基礎値になる
    [SerializeField] int resMiaMultiplier = 10;
    [SerializeField] int resDisMultiplier = 20;

    //0～その場所のMiasmaにこれをかけたものがランダムで足される
    [SerializeField] int resRandMultiplier = 1;
    public Vector2 originCoords{get{return origin;}}
    public Vector2 goalCoords{get{return goal;}}

    Vector2Int origin = new Vector2Int(50, 0);
    Vector2Int goal = new Vector2Int(50, 99);

    //スタートからゴールを指すベクトル
    Vector2Int routeVector;

    public SectorMap GenerateMap(int? seed = null)
    {

        if (seed != null)
        {
            Random.InitState(seed.Value);
        }

        if (randomizeGoal)
        {
            goal.x = Random.Range(0, 100);
        }

        routeVector = goal - origin;


        var map = new SectorMap();

        map.SetMiasma(new int[mapSize.x, mapSize.y]);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                map.miasmaMap[x, y] = GetMiasmaLv(x, y);
                SetIsland(x, y, map);
            }
        }


        InitIslands(map);

        return map;
    }

    //とりあえずMiasmaはy座標だけを指標に設定。上がり方は…二次関数
    int GetMiasmaLv(int x, int y)
    {
        var max = Mathf.Sqrt(maxMiasma);
        var normalizedVal = y / (float)mapSize.y;

        return (int)Mathf.Pow(normalizedVal, 2);
    }

    //島を配置
    //とりあえずMiasmaが高いところ程生成されづらい、とだけして、Xは指標にしない
    SectorMap SetIsland(int x, int y, SectorMap map)
    {
        var possibility = islandPossibility - maxPossibilityDiscount * map.miasmaMap[x, y] / maxMiasma;
        var dice = Random.Range(0, 100);
        if (dice < possibility)
        {
            var step = new SectorStep();
            step.radius = islandSize;
            map.TryAddStep(x, y, step);
        }

        return map;
    }

    //配置した島の情報を設定。
    //とりあえずMiasmaが高い場所ほど多く、またスタートとゴールを結んでできる線分から離れてる場所ほど多くする。
    //どっちもできるだけ顕著にする。特に後者
    SectorMap InitIslands(SectorMap map)
    {

        foreach (var step in map.mapData)
        {
            var cords = step.Key;
            var baseVol = map.miasmaMap[cords.x, cords.y] * resMiaMultiplier;

            var stepVec = step.Key - origin;
            var angle = Vector2.SignedAngle(stepVec, origin);
            angle = Mathf.Abs(angle);

            var distance = stepVec.magnitude * Mathf.Sin(angle);
            baseVol += (int)distance * resDisMultiplier;
            baseVol += Random.Range(0, resRandMultiplier * map.miasmaMap[cords.x, cords.y]);

            step.Value.resourceLv = baseVol;
        }

        return map;
    }

}