using UnityEngine;

[CreateAssetMenu(menuName = "Configs/StepGeneration")]
public class StepGenerationConfig : SingleScriptableObject<StepGenerationConfig>
{
    [SerializeField] float _gridToCanvasRate = 20f;


    public float maxMiasma{get{return _maxMiasma;}}
    [SerializeField] float _maxMiasma = 200;
    [SerializeField] Vector2Int mapSize = new Vector2Int(100, 100);
    [SerializeField] int originTilling = 2;
    [SerializeField] int goalTilling = 2;
    public float gridToCanvasrate { get { return _gridToCanvasRate; } }
    [SerializeField] int islandSize = 1;
    [SerializeField] int minGap = 3;



    //マスX,Yに島が生成される最大の確立
    [SerializeField] float islandPossibility = 0.1f;
    [SerializeField] float maxPossibilityDiscount = 0.03f;

    [SerializeField] bool randomizeGoal;

    //その場所のMiasmaにこれがかけられた値がその場所の基礎値になる
    [SerializeField] int resMiaMultiplier = 10;
    [SerializeField] int resDisMultiplier = 20;

    //0～その場所のMiasmaにこれをかけたものがランダムで足される
    [SerializeField] int resRandMultiplier = 1;
    public Vector2 originCoords { get { return origin; } }
    public Vector2 goalCoords { get { return goal; } }

    [SerializeField] Vector2Int origin = new Vector2Int(50, 0);
    [SerializeField] Vector2Int goal = new Vector2Int(50, 99);

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

        SetIsland((int)originCoords.x, (int)originCoords.y, map);
        SetIsland((int)goalCoords.x, (int)goalCoords.y, map);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                map.miasmaMap[x, y] = GetMiasmaLv(x, y);
            }
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (y > originTilling && y < mapSize.y - goalTilling)
                {
                    SetIsland(x, y, map);
                }
            }
        }


        InitIslands(map);

        return map;
    }

    //とりあえずMiasmaはy座標だけを指標に設定。上がり方は…二次関数
    int GetMiasmaLv(int x, int y)
    {
        var max = Mathf.Sqrt(_maxMiasma);
        var normalizedVal = y / (float)mapSize.y;

        return (int)Mathf.Pow(normalizedVal, 2);
    }

    //島を配置
    //とりあえずMiasmaが高いところ程生成されづらい、とだけして、Xは指標にしない
    SectorMap SetIsland(int x, int y, SectorMap map)
    {
        var possibility = islandPossibility - maxPossibilityDiscount * map.miasmaMap[x, y] / _maxMiasma;
        var dice = Random.Range(0, 100);
        if (dice < possibility)
        {
            var step = new SectorStep();
            //疑似的に最小のgapを足した数で配置
            step.radius = islandSize + minGap;
            map.TryAddStep(x, y, step);

            //その後引く
            step.radius -= minGap;
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