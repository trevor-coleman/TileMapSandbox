using System;
using System.Collections;
using System.Collections.Generic;
using Buildables;
using DefaultNamespace;
using Tools;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public enum Buildable
{
    Station,
    Track,
    All
}

public class Builder : MonoBehaviour
{
    private WorldMap worldMap;
    private GameObject builtObjectParent;
    private MoneyManager moneyManager;
    private GameManager gameManager;


    public enum BuildingResult
    {
        Init,
        Success,
        InsufficientFunds,
        SpaceBlocked,
        IllegalPlacement,
        NothingThere
    }

    [SerializeField] private GameObject stationPrefab;
    [SerializeField] private GameObject trackPrefab;
    [SerializeField] private GameObject trackStubPrefab;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = FindObjectOfType<WorldMap>();
        builtObjectParent = new GameObject("Built Objects");
        moneyManager = FindObjectOfType<MoneyManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BuildStation(StationData stationData, out BuildingResult result)
    {
        Tile<TileData> tile = stationData.Tile;

        GameObject newStationObject = Instantiate(stationPrefab, tile.CartesianPosition, Quaternion.identity,
            builtObjectParent.transform);

        Station newStation = newStationObject.GetComponent<Station>();

        newStation.UpdateData(stationData);


        tile.Data.HasStation = true;
        tile.Data.Station = newStation;
        newStationObject.GetComponent<Station>().SetTile(tile);

        AddTracksToTile(tile);

        worldMap.AddBuiltObject(tile.Position, newStationObject);
        List<Station> transitSystemStations = gameManager.transitSystem.Stations;
        transitSystemStations.Add(newStation);

        gameManager.transitSystem.UpdateAllStationConnections();

        result = BuildingResult.Success;
    }

    public void BuildTrack(TrackData trackData, out BuildingResult result)
    {
        Tile<TileData> tile = trackData.Tile;

        AddTracksToTile(tile);

        gameManager.transitSystem.UpdateAllStationConnections();

        result = BuildingResult.Success;
    }

    private void AddTracksToTile(Tile<TileData> tile)
    {
        tile.Data.HasTracks = true;
        
        List<Tile<TileData>> neighborTilesWithTracks = GetNeighborTilesWithTracks(tile);

        if (neighborTilesWithTracks.Count > 0)
        {
            foreach (Tile<TileData> neighborTile in neighborTilesWithTracks)
            {
                GameObject trackStub;

                if (neighborTile.Data.TrackObjectsByNeighborPosition.TryGetValue(neighborTile.Position, out trackStub))
                    Destroy(trackStub);

                InstantiateTrackObject(tile, neighborTile);
                InstantiateTrackObject(neighborTile, tile);
            }
        }
        else
        {
            tile.Data.TrackStubObject =
                Instantiate(
                    trackStubPrefab,
                    tile.CartesianPosition,
                    Quaternion.identity,
                    builtObjectParent.transform
                );
        }
    }

    private void InstantiateTrackObject(Tile<TileData> tile, Tile<TileData> neighborTile)
    {

        Vector3 edgePosition =
            worldMap
                .HexMap
                .GetEdge
                .BetweenNeighbouringTiles(
                    tile.Position,
                    neighborTile.Position)
                .CartesianPosition;

        Quaternion trackRotation = Quaternion.FromToRotation(Vector3.forward, edgePosition - tile.CartesianPosition);


        GameObject trackObject = Instantiate(
            trackPrefab,
            tile.CartesianPosition,
            trackRotation,
            builtObjectParent.transform);

        tile.Data.TrackObjectsByNeighborPosition[neighborTile.Position] = trackObject;
        worldMap.AddBuiltObject(tile.Position, trackObject);
    }

    public void Demolish(IBuildableData demolishData)
    {
        BuildingResult result = BuildingResult.Init;
        if (moneyManager.CanSpend(demolishData.Price))
        {
            switch (demolishData.Buildable)
            {
                case Buildable.Station:
                    DemolishStation(demolishData as StationData, out result);
                    break;
                case Buildable.Track:
                    DemolishTrack(demolishData as TrackData, out result);
                    break;
                case Buildable.All:
                    DemolishAll(demolishData, out result);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;
            }
        }
        else
        {
            result = BuildingResult.InsufficientFunds;
        }

        switch (result)
        {
            case BuildingResult.Success:
                moneyManager.Spend(demolishData.Price);
                break;
            case BuildingResult.InsufficientFunds:
            case BuildingResult.SpaceBlocked:
            case BuildingResult.IllegalPlacement:
            case BuildingResult.NothingThere:
                Debug.LogWarning("Demolish Failed - " + demolishData.Buildable + " - " + result);
                break;
            default:
                throw new ArgumentOutOfRangeException();
                break;
        }


    }

    public void Build(IBuildableData buildableData)
    {
        BuildingResult result = BuildingResult.Init;

        if (moneyManager.CanSpend(buildableData.Price))
        {
            switch (buildableData.Buildable)
            {
                case Buildable.Station:
                    BuildStation(buildableData as StationData, out result);
                    break;
                case Buildable.Track:
                    BuildTrack(buildableData as TrackData, out result);
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


    private void DemolishTrack(IBuildableData demolishData, out BuildingResult result)
    {
        Tile<TileData> tile = demolishData.Tile;

        RemoveTracksFromTile(tile);
        
        gameManager.transitSystem.UpdateAllStationConnections();


        result = BuildingResult.Success;
    }
    
    private void RemoveTracksFromTile(Tile<TileData> tile)
    {
        tile.Data.HasTracks = false;

        List<Tile<TileData>> neighborTilesWithTracks = GetNeighborTilesWithTracks(tile);
        
        if (neighborTilesWithTracks.Count == 0)
        {
            Destroy(tile.Data.TrackStubObject);
            return;
        }
        
        foreach (Tile<TileData> neighborTile in neighborTilesWithTracks)
        {
            Destroy(tile.Data.TrackObjectsByNeighborPosition[neighborTile.Position]);
            Destroy(neighborTile.Data.TrackObjectsByNeighborPosition[tile.Position]);
            
        }
    }

    private List<Tile<TileData>> GetNeighborTilesWithTracks(Tile<TileData> tile)
    {
        List<Tile<TileData>> neighborTiles = worldMap.HexMap.GetTiles.Ring(tile, 1, 1);
        List<Tile<TileData>> neighborTilesWithTracks = new List<Tile<TileData>>();
        List<float> trackAngles = new List<float>();

        foreach (Tile<TileData> neighborTile in neighborTiles)
        {
            if (neighborTile.Data.HasTracks)
            {
                neighborTilesWithTracks.Add(neighborTile);
            }
        }

        return neighborTilesWithTracks;
    }


    private void DemolishStation(StationData stationData, out BuildingResult result)
    {
        throw new NotImplementedException();
    }
    
    private void DemolishAll(IBuildableData demolishData, out BuildingResult result)
    {
        throw new NotImplementedException();
    }
}