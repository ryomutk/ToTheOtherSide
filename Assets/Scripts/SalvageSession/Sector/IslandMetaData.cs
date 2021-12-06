using UnityEngine;

public interface IMetaIsland
{
    Vector2 rawCoordinate{get;}
    Vector2 localCoordinate{get;}
    string name{get;}
    StepState state{get;}
    int miasma{get;}
    int barrier{get;}
    /// <summary>
    /// MOTHERからのきょり
    /// </summary>
    /// <value></value>
    float metorDistanceFM{get;}
}