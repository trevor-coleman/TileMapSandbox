using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class TileData
{
    public int population;
    public TileType Type;
    public bool Populated => population > 0;
    

    public bool HasStation { get; set; }
    public bool HasTracks { get; set; }
    public Dictionary<Vector3Int, GameObject> TrackObjectsByNeighborPosition;
    public GameObject StationObject { get; set; }
    public GameObject TrackStubObject { get; set; }
    
    public TileData()
    {
        this.population = 0;
        this.Type = TileType.Pasture;
        this.HasStation = false;
        this.HasTracks = false;
        this.TrackObjectsByNeighborPosition = new Dictionary<Vector3Int, GameObject>();
        this.StationObject = null;
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