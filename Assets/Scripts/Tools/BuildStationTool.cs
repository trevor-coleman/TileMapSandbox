using System.Collections.Generic;
using Buildables;
using TMPro;
using UI;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class BuildStationTool : MonoBehaviour
{
    private WorldMap worldMap;
    private ToolManager toolManager;
    private bool cleanedUp;

    private TextMeshPro stationMarkerText;
    [SerializeField] private GameObject stationPrefab;
    private GameObject stationMarker;

    private HexMap<TileData, EdgeData> hexMap;
    [SerializeField] private GameObject catchmentMarkerPopulatedPrefab;
    [SerializeField] private GameObject catchmentMarkerEmptyPrefab;
    private List<GameObject> catchmentMarkers;
    [SerializeField] private Vector3Int previousMouseTilePosition;
    private Dictionary<Vector3Int, GameObject> catchmentMarkerObjectsByPosition;
    private Tile<TileData> mouseTile;
    [SerializeField] private int stationRange = 1;
    private UIController uiController;
    private GameObject catchmentMarkerParentObject;
    private bool catchmentParentObjectInstantiated;
    private Builder builder;
    private MoneyManager moneyManager;
    private StationMarkerCylinder stationMarkerCylinder;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = FindObjectOfType<WorldMap>();
        builder = FindObjectOfType<Builder>();
        toolManager = FindObjectOfType<ToolManager>();
        hexMap = worldMap.HexMap;
        catchmentMarkers = new List<GameObject>();
        uiController = FindObjectOfType<UIController>();
        MakeCatchmentParentObject();
    }

    private void MakeCatchmentParentObject()
    {
        catchmentMarkerParentObject = new GameObject
        {
            name = "Catchment Markers",
            transform =
            {
                parent = gameObject.transform
            }
        };
        catchmentParentObjectInstantiated = true;
    }

    private void DestroyCatchmentParentObject()
    {
        Destroy(catchmentMarkerParentObject);
        catchmentParentObjectInstantiated = false;
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

        if (worldMap.MouseTilePosition != previousMouseTilePosition)
        {
            mouseTile = hexMap.TilesByPosition[worldMap.MouseTilePosition];

            UpdateStationMarker(worldMap.MouseTilePosition);
            UpdateCatchmentMarkers();


            UpdateStationBuilderOverlay();
        }

        if (Input.GetMouseButtonDown(0))
        {
            BuildCommand<StationData> buildStation =
                new BuildCommand<StationData>(builder, new StationData(mouseTile, 1));
            builder.ExecuteCommand(buildStation);
        }

        previousMouseTilePosition = worldMap.MouseTilePosition;
    }

    private int CatchmentPopulation()
    {
        int catchmentPopulation = 0;

        foreach (Tile<TileData> catchmentTile in CatchmentTiles())
        {
            catchmentPopulation += catchmentTile.Data.population;
        }

        return catchmentPopulation;
    }

    private void UpdateStationBuilderOverlay()
    {
        BuildStationOverlayProperties data = new BuildStationOverlayProperties {population = CatchmentPopulation()};

        ToolOverLayProperties<BuildStationOverlayProperties> properties =
            new ToolOverLayProperties<BuildStationOverlayProperties> {Tool = Tool.BuildStation, Data = data};

        uiController.UpdateToolOverlay(properties);
    }

    private List<Tile<TileData>> CatchmentRingTiles()
    {
        return hexMap.GetTiles.Disc(mouseTile, stationRange, false);
        ;
    }

    private List<Tile<TileData>> CatchmentTiles()
    {
        return hexMap.GetTiles.Disc(mouseTile, stationRange, true);
    }


    private void UpdateCatchmentMarkers()
    {
        RemoveCatchmentMarkers();
        
        if (mouseTile.Data.hasStation) return;

        if (!catchmentParentObjectInstantiated)
        {
            MakeCatchmentParentObject();
        }
        
        foreach (Tile<TileData> catchmentRingTile in CatchmentRingTiles())
        {
            if (catchmentRingTile.Data.hasStation) continue;

            GameObject thisTileMarker = catchmentRingTile.Data.Populated
                ? catchmentMarkerPopulatedPrefab
                : catchmentMarkerEmptyPrefab;

            GameObject catchmentMarker = Instantiate(
                thisTileMarker,
                HexConverter.TileCoordToCartesianCoord(catchmentRingTile.Position, 0.1f),
                Quaternion.identity,
                catchmentMarkerParentObject.transform
            );
            catchmentMarkers.Add(catchmentMarker);
        }
    }

    private void UpdateStationMarker(Vector3Int mouseTilePosition)
    {
        if (stationMarker == null)
        {
            stationMarker = Instantiate(stationPrefab, transform);
            stationMarker.name = "StationBuilder - StationMarker";
            stationMarkerText = stationMarker.GetComponentInChildren<TextMeshPro>();
            stationMarkerCylinder = stationMarker.GetComponentInChildren<StationMarkerCylinder>();
        }

        if (mouseTile.Data.hasStation)
        {
            Debug.Log("STATION");
            stationMarkerCylinder.currentState = StationMarkerCylinder.State.Prohibited;
        }
        else
        {
            stationMarkerCylinder.currentState = StationMarkerCylinder.State.Default;
        }
        
        stationMarker.transform.position = HexConverter.TileCoordToCartesianCoord(mouseTilePosition, 0.1f);
        stationMarkerText.text = CatchmentPopulation().ToString();
    }


    private void CleanUp()
    {
        DestroyCatchmentParentObject();
        uiController.CancelTool(Tool.BuildStation);
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