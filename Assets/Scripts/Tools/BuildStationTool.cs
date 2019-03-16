using System.Collections.Generic;
using Buildables;
using UI;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class BuildStationTool : MonoBehaviour
{
    private WorldMap worldMap;
    private ToolManager toolManager;
    private bool cleanedUp;


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

    private void UpdateStationBuilderOverlay()
    {
        int catchmentPopulation = 0;

        catchmentPopulation += mouseTile.Data.population;

        foreach (Tile<TileData> catchmentTile in CatchmentTiles())
        {
            catchmentPopulation += catchmentTile.Data.population;
        }

        BuildStationOverlayProperties data = new BuildStationOverlayProperties {population = catchmentPopulation};

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
        if (!catchmentParentObjectInstantiated)
        {
            MakeCatchmentParentObject();
        }

        RemoveCatchmentMarkers();

        foreach (Tile<TileData> catchmentTile in CatchmentRingTiles())
        {
            if (catchmentTile.Data.hasStation) continue;

            GameObject thisTileMarker = catchmentTile.Data.Populated
                ? catchmentMarkerPopulatedPrefab
                : catchmentMarkerEmptyPrefab;

            GameObject catchmentMarker = Instantiate(
                thisTileMarker,
                HexConverter.TileCoordToCartesianCoord(catchmentTile.Position, 0.1f),
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
        }

        stationMarker.transform.position = HexConverter.TileCoordToCartesianCoord(mouseTilePosition, 0.1f);
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