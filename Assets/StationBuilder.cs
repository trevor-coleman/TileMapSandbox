﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class StationBuilder : MonoBehaviour
{
    private WorldMap worldMap;
    private ToolManager toolManager;
    private bool cleanedUp;
    

    [SerializeField] private GameObject stationPrefab;
    private GameObject stationMarker;
    
    private HexMap<int,bool> hexMap;
    [SerializeField] private GameObject catchmentMarkerPopulatedPrefab;
    [SerializeField] private GameObject catchmentMarkerEmptyPrefab;
    private List<GameObject> catchmentMarkers;
    [SerializeField] private Vector3Int previousMouseTilePosition;
    private Dictionary<Vector3Int, GameObject> catchmentMarkerObjectsByPosition;
    private Tile<int> hoverTile;
    [SerializeField] private int stationRange = 1;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = FindObjectOfType<WorldMap>();
        toolManager = FindObjectOfType<ToolManager>();
        hexMap = worldMap.hexMap;
        catchmentMarkers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (toolManager.ActiveTool != Tool.BuildStation)
        {
            if (cleanedUp) return;

            CleanUp();
            return;
        }


        cleanedUp = false;

        if (worldMap.mouseTilePosition != previousMouseTilePosition)
        {

            hoverTile = hexMap.TilesByPosition[worldMap.mouseTilePosition];
            
            UpdateStationMarker(worldMap.mouseTilePosition);
            List<Tile<int>> catchmentMarkerPositions = CalculateCatchmentArea(hoverTile, stationRange); 
            UpdateCatchmentMarkers(catchmentMarkerPositions);
            
        }

        previousMouseTilePosition = worldMap.mouseTilePosition;

    }

    private List<Tile<int>> CalculateCatchmentArea(Tile<int> origin, int stationRange = 1)
    {
        List<Tile<int>> ringTiles = hexMap.GetTiles.Disc(origin, stationRange, false);
        List<Tile<int>> catchmentTiles = new List<Tile<int>>();

        foreach (Tile<int> ringTile in ringTiles)
        {
            
            catchmentTiles.Add(ringTile);
        }

        return catchmentTiles;
    }
    
    private void UpdateCatchmentMarkers(IEnumerable<Tile<int>> catchmentTiles)
    {
        RemoveCatchmentMarkers();

        foreach (Tile<int> catchmentTile in catchmentTiles)
        {
            GameObject thisTileMarker = null;
            
            GameObject catchmentTileObj = worldMap.tileObjects[catchmentTile.Index];
            thisTileMarker = catchmentTileObj.GetComponent<WorldTile>() is IPopulated
                ? catchmentMarkerPopulatedPrefab
                : catchmentMarkerEmptyPrefab; 
            
            GameObject catchmentMarker = Instantiate(thisTileMarker,
                HexConverter.TileCoordToCartesianCoord(catchmentTile.Position, 0.1f), Quaternion.identity);
            catchmentMarkers.Add(catchmentMarker);
        }
        
    }

    private void UpdateStationMarker(Vector3Int mouseTilePosition)
    {
        
        if (stationMarker == null)
        {
            stationMarker = Instantiate(stationPrefab, transform);
            stationMarker.name = "StationBuilder - StationMarker";
        }
        else
        {
            stationMarker.transform.position = HexConverter.TileCoordToCartesianCoord(mouseTilePosition, 0.1f);
        }
    }


    private void CleanUp()
    {

        RemoveCatchmentMarkers();
        Destroy(stationMarker);
        cleanedUp = true;
    }

    private void RemoveCatchmentMarkers()
    {
        foreach (GameObject go in catchmentMarkers)
        {
            Destroy(go);
        }

        catchmentMarkers.Clear();
    }
}