using System;
using System.Collections;
using System.Collections.Generic;
using Buildables;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public enum Buildable
{
    Station,
}

public class Builder : MonoBehaviour
{
    private WorldMap worldMap;
    private GameObject builtObjectParent;

    [SerializeField]private GameObject stationPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        worldMap = FindObjectOfType<WorldMap>();
        builtObjectParent = new GameObject("Built Objects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BuildStation(StationData stationData)
    {
        Tile<TileData> tile = stationData.Tile;
        
        GameObject newStation = Instantiate(stationPrefab, tile.CartesianPosition, Quaternion.identity,
            builtObjectParent.transform);

        newStation.GetComponent<Station>().UpdateData(stationData);

        tile.Data.hasStation = true;

        worldMap.AddBuiltObject(tile.Position, newStation);
    }

    public void Build(IBuildableData buildableData)
    {    
        switch (buildableData.Buildable)
        {
            case Buildable.Station:
                BuildStation(buildableData as StationData);
                break;
            
        }
    }


    public void ExecuteCommand(IBuildCommand buildCommand)
    {
        buildCommand.Execute();
    }
}

