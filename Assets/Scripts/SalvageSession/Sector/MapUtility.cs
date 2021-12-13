using UnityEngine;

public static class MapUtility
{
    class IslandMetaData : IMetaIsland
    {
        public Vector2 rawCoordinate { get;set; }
        public Vector2 localCoordinate { get; set;}
        public string name { get; set;}
        public StepState state { get;set; }
        public int miasma { get; set;}
        public int barrier { get; set;}
        public float metorDistanceFM { get;set; }
    }
    
    public static IMetaIsland GetMetaData(int islandID)
    {
        var island= DataProvider.nowGameData.map.GetIsland(islandID);
        return GetMetaData(island);
    }
    public static IMetaIsland GetMetaData(Island island)
    {
        var data = new IslandMetaData();
        data.rawCoordinate = DataProvider.nowGameData.map.GetCoordinate(island);
        data.localCoordinate = data.rawCoordinate - StepGenerationConfig.instance.originCoords;
        data.name = island.name;
        //data.state = 

        var (x,y) = ((int)data.rawCoordinate.x,(int)data.localCoordinate.y);
        data.miasma = DataProvider.nowGameData.map.miasmaMap[x,y];

        //MOTHERからの情報はちょっとあれかも。
        data.metorDistanceFM = (data.rawCoordinate-DataProvider.nowGameData.currentMOTHERCoordinate).magnitude*StepGenerationConfig.instance.gridToMetor;

        return data;
    }

    public static int GetMiasma(Vector2 coordinate)
    {
        var coords = Vector2Int.FloorToInt(coordinate);
        return DataProvider.nowGameData.map.miasmaMap[coords.x,coords.y];
    }

    public static float GetMiasmaDamage(Vector2 coordnate)
    {
        return GetMiasma(coordnate)*StepGenerationConfig.instance.miasmaDamageRate;     
    }
}