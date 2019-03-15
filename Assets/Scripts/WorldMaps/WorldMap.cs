using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using Random = UnityEngine.Random;

public class WorldMap : MonoBehaviour
{
    [Header("Camera")] 
    [SerializeField] protected bool adjustCameraToFit;


    [Header("Map Settings")] [SerializeField]
    protected int mapRadius;
    [SerializeField] private GameObject grassTilePrefab;

    protected internal GameObject[] tileObjects;
    public HexMap<int, bool> hexMap { get; private set; }
    protected HexMouse hexMouse;
    
    public Vector3Int mouseTilePosition { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        mouseTilePosition = hexMouse.TileCoord;
        
    }

    protected void Init()
    {
        
        MakeHexMap();
        
        hexMouse = GetComponent<HexMouse>();
        
        if (hexMouse == null)
        {
            hexMouse = gameObject.AddComponent<HexMouse>();
        }
        
        hexMouse.Init(hexMap);
    }

    protected void MakeHexMap()
    {
        hexMap = new HexMap<int, bool>(HexMapBuilder.CreateHexagonalShapedMap(mapRadius));
        tileObjects = new GameObject[hexMap.TilesByPosition.Count];

        foreach (Tile<int> tile in hexMap.Tiles)
        {
            InstantiateNewTerrainTile(grassTilePrefab, tile);
            tile.Data = 0;
        }
    }

    protected GameObject ReplaceTerrainTile(Tile<int> tile, GameObject tilePrefab)
    {
        Destroy(tileObjects[tile.Index]);
        GameObject newGameObject = InstantiateNewTerrainTile(tilePrefab, tile);
        return newGameObject;
    }
    
    protected GameObject InstantiateNewTerrainTile(GameObject tilePrefab, Tile<int> tile)
    {
        GameObject instance = Instantiate(tilePrefab, transform, true);
        instance.transform.position = tile.CartesianPosition;
        instance.name = "Tile - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);
        tileObjects[tile.Index] = instance;
        return instance;
    }

    protected static Vector3 RandomHexRotation()
    {
        

        int degrees = Random.Range(0, 6) * 60;
        
        return new Vector3(0, degrees, 0); 
    }

    protected void AdjustCameraToFit()
    {
        Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z);
        Camera.main.orthographic = true;
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
        Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f;
    }
}