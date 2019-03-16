using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public int population;
    public TileType Type;
    public bool Populated => population > 0;
    public bool hasStation;
    
    
    public TileData()
    {
        this.population = 0;
        this.Type = TileType.Pasture;
        this.hasStation = false;
    }
}


public enum TileType
{
    Pasture,
    Suburb,
    City    
}

public class EdgeData
{
    public EdgeData()
    {
        
    }
}


public class CornerData
{
    public CornerData()
    {
        
    }
}