using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using Random = UnityEngine.Random;

public class HelloHexWorld : MonoBehaviour
{
    [SerializeField] private int mapRadius = 8;
    [SerializeField] private TerrainTileSet terrainTileSet;
    [SerializeField] private GameObject tileMarker;
    [SerializeField] private GameObject edgeMarker;
    [SerializeField] private GameObject cornerMarker;
    [SerializeField] private GameObject lineMarker;


    private List<GameObject> visionMarkers;
    private List<GameObject> lineMarkers;
    private HexMouse hexMouse;
    private GameObject[] tileObjects;
    private HexMap<int, bool> hexMap;

    public enum TerrainType
    {
        Clay,
        Coal,
        Desert,
        Diamond,
        Gold,
        Iron,
        Lake,
        Pasture,
        River,
        Stone,
        Wasteland,
        Water
    }

    public Hashtable typeToPrefab;
    [SerializeField] private bool showTileMarker;
    [SerializeField] private bool showEdgeMarker;
    [SerializeField] private bool showCornerMarker;
    [SerializeField] private GameObject tileVisionMarker;
    [SerializeField] private GameObject edgeVisionBorder;
    private Tile<int> playerPosition;
    private Tile<int> secondTile;
    private bool playerOnBoard;
    private bool secondTileIsSelected;
    private bool firstClickMarkerSet;
    private bool showingVisionMarkers;
    private bool playerIsMoving;
    private Tile<int> previousTile;

    // Start is called before the first frame update
    void Start()
    {
        lineMarkers = new List<GameObject>();
        MakeTerrainHashtable();

        MakeHexMap();

        hexMouse = GetComponent<HexMouse>();
        hexMouse.Init(hexMap);


        AdjustCameraToFit();
    }

    void Update()
    {
        
        MovePlayerToken(playerPosition);
        if (playerIsMoving) return;
        
        if (!hexMouse.CursorIsOnMap) return;

        Vector3Int mouseTilePosition = hexMouse.TileCoord;
        Vector3Int mouseEdgePosition = hexMouse.ClosestEdgeCoord;
        Vector3Int mouseCornerPosition = hexMouse.ClosestCornerCoord;

//        UpdateTileMarker(mouseTilePosition);
//        UpdateEdgeMarker(mouseEdgePosition);
//        UpdateCornerMarker(mouseCornerPosition);

        if (Input.GetMouseButtonDown(1))
        {
            if (!showingVisionMarkers)
            {
                if (playerOnBoard && mouseTilePosition == playerPosition.Position
                    || secondTileIsSelected && mouseTilePosition == secondTile.Position)
                {
                    HashSet<Vector3Int> visibleTiles = CalculateVisibleTiles(mouseTilePosition, 5);
                    UpdateVisionMarkers(visibleTiles);
                    showingVisionMarkers = true;
                }
                else if (!playerOnBoard)
                {
                    HashSet<Vector3Int> visibleTiles = CalculateVisibleTiles(mouseTilePosition, 5);
                    UpdateVisionMarkers(visibleTiles);
                    showingVisionMarkers = true;
                }
            }
            else
            {
                ClearVisionMarkers();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (showingVisionMarkers) ClearVisionMarkers();

            Tile<int> tile = hexMap.TilesByPosition[mouseTilePosition];

            if (!playerOnBoard)
            {
                playerPosition = tile;
                playerOnBoard = true;
            }
            else
            {
                if (tile == playerPosition)
                {
                    ResetLine();
                    ClearVisionMarkers();
                }
                else
                {
                    if (secondTile == tile)
                    {
                        playerPosition = secondTile;
                        playerOnBoard = true;
                        secondTileIsSelected = false;
                        secondTile = null;
                        ClearLineMarkers();
                    }
                    else
                    {
                        ClearLineMarkers();
                        secondTile = tile;
                        secondTileIsSelected = true;
                    }
                }
            }

            

            if (secondTileIsSelected)
            {
                DrawLine(playerPosition, secondTile);
            }


//            ReplaceTileWithRandomTerrainTile(tile);
        }
    }

    private void ResetLine()
    {
        playerPosition = null;
        playerOnBoard = false;
        secondTile = null;
        secondTileIsSelected = false;
        ClearLineMarkers();
    }

    private void ClearLineMarkers()
    {
        foreach (GameObject g in lineMarkers)
        {
            Destroy(g);
        }

        lineMarkers.Clear();
    }

    private void DrawLine(Tile<int> origin, Tile<int> destination)
    {
        List<Tile<int>> lineA = hexMap.GetTiles.Line(origin.Position, destination.Position, true, +0.001f);
        List<Tile<int>> lineB = hexMap.GetTiles.Line(origin.Position, destination.Position, true, -0.001f);

        List<Tile<int>> shortestLine = lineA.Count <= lineB.Count ? lineA : lineB;

        foreach (Tile<int> tile in shortestLine)
        {
            GameObject tileObj = Instantiate(lineMarker,
                HexConverter.TileCoordToCartesianCoord(tile.Position, 0.1f), Quaternion.identity);
            lineMarkers.Add(tileObj);
        }
    }


    private void MakeHexMap()
    {
        hexMap = new HexMap<int, bool>(HexMapBuilder.CreateHexagonalShapedMap(mapRadius));
        tileObjects = new GameObject[hexMap.TilesByPosition.Count];
        visionMarkers = new List<GameObject>();

        foreach (Tile<int> tile in hexMap.Tiles)
        {
            var types = Enum.GetValues(typeof(TerrainType));
            Random random = new Random();
            TerrainType randomTerrainType = (TerrainType) types.GetValue(Random.Range(0, types.Length));

            GameObject tilePrefab = GetPrefab(randomTerrainType);
            Debug.Log(tilePrefab + " --- " + tile);
            InstantiateNewTerrainTile(tilePrefab, tile);
            tile.Data = (int) randomTerrainType;
        }
    }

    private void InstantiateNewTerrainTile(GameObject tilePrefab, Tile<int> tile)
    {
        GameObject instance = Instantiate(tilePrefab, transform, true);
        instance.transform.position = tile.CartesianPosition;
        instance.name = "Tile - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);
        tileObjects[tile.Index] = instance;
    }

    private HashSet<Vector3Int> CalculateVisibleTiles(Vector3Int origin, int range)
    {
        List<Vector3Int> ringTiles = HexGrid.GetTiles.Ring(origin, range, 1);
        HashSet<Vector3Int> reachedTiles = new HashSet<Vector3Int>();

        foreach (Vector3Int ringTile in ringTiles)
        {
            List<Tile<int>> lineA = hexMap.GetTiles.Line(origin, ringTile, true, +0.001f);
            List<Tile<int>> lineB = hexMap.GetTiles.Line(origin, ringTile, true, -0.001f);

            List<List<Tile<int>>> lines = new List<List<Tile<int>>> {lineA, lineB};
            foreach (List<Tile<int>> line in lines)
            {
                for (int i = 0; i < line.Count; i++)
                {
                    reachedTiles.Add(line[i].Position);
                    if (line[i].Data == 2 || line[i].Data >= 9) break;
                }
            }
        }

        return reachedTiles;
    }

    private void UpdateVisionMarkers(IEnumerable<Vector3Int> visibleTiles)
    {
        ClearVisionMarkers();

        
        
        foreach (Vector3Int tilePos in visibleTiles)
        {
            GameObject visionObj = Instantiate(tileVisionMarker,
                HexConverter.TileCoordToCartesianCoord(tilePos, 0.1f), Quaternion.identity);
            visionMarkers.Add(visionObj);
        }

        List<Vector3Int> borderEdges = hexMap.GetEdgePositions.TileBorders((visibleTiles));

        foreach (Vector3Int edgePos in borderEdges)
        {
            EdgeAlignment orientation = HexUtility.GetEdgeAlignment(edgePos);
            float angle = HexUtility.anglebyEdgeAlignment[orientation];
            GameObject edgeObj = GameObject.Instantiate(
                edgeVisionBorder,
                HexConverter.EdgeCoordToCartesianCoord(edgePos),
                Quaternion.Euler(0, angle, 0)
            );
            visionMarkers.Add(edgeObj);
        }
    }

    private void ClearVisionMarkers()
    {
        foreach (GameObject go in visionMarkers)
        {
            Destroy(go);
        }

        visionMarkers.Clear();
        showingVisionMarkers = false;
    }

    private void MakeTerrainHashtable()
    {
        typeToPrefab = new Hashtable
        {
            {TerrainType.Clay, terrainTileSet.clayPrefab},
            {TerrainType.Coal, terrainTileSet.coalPrefab},
            {TerrainType.Desert, terrainTileSet.desertPrefab},
            {TerrainType.Gold, terrainTileSet.goldPrefab},
            {TerrainType.Diamond, terrainTileSet.diamondPrefab},
            {TerrainType.Iron, terrainTileSet.ironPrefab},
            {TerrainType.Lake, terrainTileSet.lakePrefab},
            {TerrainType.Pasture, terrainTileSet.pasturePrefab},
            {TerrainType.River, terrainTileSet.riverPrefab},
            {TerrainType.Stone, terrainTileSet.stonePrefab},
            {TerrainType.Wasteland, terrainTileSet.wastelandPrefab},
            {TerrainType.Water, terrainTileSet.waterPrefab}
        };
    }

    public GameObject RandomTerrain()
    {
        var types = Enum.GetValues(typeof(TerrainType));
        Random random = new Random();
        TerrainType randomTerrainType = (TerrainType) types.GetValue(Random.Range(0, types.Length));
        return (GameObject) typeToPrefab[randomTerrainType];
    }

    public GameObject GetPrefab(TerrainType terrainType)
    {
        return (GameObject) typeToPrefab[terrainType];
    }

    private void ReplaceTileWithRandomTerrainTile(Tile<int> tile)
    {
        DestroyImmediate(tileObjects[tile.Index]);
        InstantiateNewTerrainTile(RandomTerrain(), tile);
    }

    private void UpdateCornerMarker(Vector3Int mouseCornerPosition)
    {
        if (showCornerMarker)
        {
            cornerMarker.SetActive(true);
            cornerMarker.transform.position = HexConverter.CornerCoordToCartesianCoord(mouseCornerPosition, 0.1f);
        }
        else
        {
            cornerMarker.SetActive(false);
        }
    }

    private void UpdateEdgeMarker(Vector3Int mouseEdgePosition)
    {
        if (showEdgeMarker)
        {
            edgeMarker.SetActive(true);
            edgeMarker.transform.position = HexConverter.EdgeCoordToCartesianCoord(mouseEdgePosition, 0.1f);
        }
        else
        {
            edgeMarker.SetActive(false);
        }
    }

    private void MovePlayerToken(Tile<int> tile)
    {
        if (!playerOnBoard)
        {
            firstClickMarkerSet = false;
            tileMarker.SetActive(false);
            playerIsMoving = false;
            return;
        }
        
        tileMarker.SetActive(true);
        if (!firstClickMarkerSet)
        {
            tileMarker.transform.position = HexConverter.TileCoordToCartesianCoord(tile.Position);
            firstClickMarkerSet = true;
            return;
        }
        
        if (tileMarker.transform.position != HexConverter.TileCoordToCartesianCoord(tile.Position))
        {
            tileMarker.transform.position =
                Vector3.MoveTowards(transform.position, HexConverter.TileCoordToCartesianCoord(tile.Position), 1.0f * Time.deltaTime);
            playerIsMoving = true;
            
        }
        else
        {
            playerIsMoving = false;
        }
        
       
    }

    private void UpdateTileMarker(Vector3Int mouseTilePosition)
    {
        if (showTileMarker)
        {
            tileMarker.SetActive(true);
            tileMarker.transform.position = HexConverter.TileCoordToCartesianCoord(mouseTilePosition, 0.1f);
        }
        else
        {
            tileMarker.SetActive(false);
        }
    }

    private void AdjustCameraToFit()
    {
        Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z);
        Camera.main.orthographic = true;
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
        Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f;
    }
}