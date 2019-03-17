using System;
using System.Collections;
using System.Collections.Generic;
using Buildables;
using UnityEditor.Animations;
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
    private MoneyManager moneyManager;
    
    public enum BuildingResult
    {
        Init,
        Success,
        InsufficientFunds,
        SpaceBlocked,
        IllegalPlacement
    }

    [SerializeField] private GameObject stationPrefab;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = FindObjectOfType<WorldMap>();
        builtObjectParent = new GameObject("Built Objects");
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public BuildingResult BuildStation(StationData stationData)
    {
        Tile<TileData> tile = stationData.Tile;

        GameObject newStation = Instantiate(stationPrefab, tile.CartesianPosition, Quaternion.identity,
            builtObjectParent.transform);

        newStation.GetComponent<Station>().UpdateData(stationData);

        tile.Data.hasStation = true;

        worldMap.AddBuiltObject(tile.Position, newStation);
        
        return BuildingResult.Success;
    }

    public void Build(IBuildableData buildableData)
    {
        BuildingResult result = BuildingResult.Init;

        if (moneyManager.CanSpend(buildableData.Price))
        {
            switch (buildableData.Buildable)
            {
                case Buildable.Station:
                    result = BuildStation(buildableData as StationData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
        }
        else
        {
            result = BuildingResult.InsufficientFunds;
        }


        switch (result)
        {
            case BuildingResult.Success:
                moneyManager.Spend(buildableData.Price);
                break;
            case BuildingResult.InsufficientFunds:
            case BuildingResult.SpaceBlocked:
            case BuildingResult.IllegalPlacement:
                Debug.LogWarning("Build Failed - " + buildableData.Buildable + " - " + result);
                break;
            default:      
                Debug.LogError("BuildResult was not properly set");
                break;
        }
    }


    public void ExecuteCommand(IBuildCommand buildCommand)
    {
        buildCommand.Execute();
    }
}