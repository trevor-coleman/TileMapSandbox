using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
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

    protected internal GameObject[] TileObjects;
    public HexMap<TileData, EdgeData> HexMap { get; private set; }
    protected HexMouse HexMouse;
    public Dictionary<Vector3Int, List<GameObject>> builtObjectsByPosition;
    public List<GameObject> builtObjects;
    public Vector3Int MouseTilePosition { get; protected set; }
    public Tile<TileData> MouseTile;
    public Builder Builder { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HexMouse.CursorIsOnMap) return;
        MouseTilePosition = HexMouse.TileCoord;
        MouseTile = HexMap.TilesByPosition[MouseTilePosition];
    }

    protected void Init()
    {
        
        MakeHexMap();
        
        HexMouse = GetComponent<HexMouse>();
        
        if (HexMouse == null)
        {
            HexMouse = gameObject.AddComponent<HexMouse>();
        }
        
        HexMouse.Init(HexMap);
        
        builtObjectsByPosition = new Dictionary<Vector3Int, List<GameObject>>();
        builtObjects = new List<GameObject>();
    }

    protected void MakeHexMap()
    {
        HexMap = new HexMap<TileData, EdgeData>(HexMapBuilder.CreateRectangularShapedMap(new Vector2Int(60,30)));
        TileObjects = new GameObject[HexMap.TilesByPosition.Count];

        foreach (Tile<TileData> tile in HexMap.Tiles)
        {
            InstantiateNewTerrainTile(grassTilePrefab, tile);
        }
    }

    protected GameObject ReplaceTerrainTile(Tile<TileData> tile, GameObject tilePrefab)
    {
        Destroy(TileObjects[tile.Index]);
        GameObject newGameObject = InstantiateNewTerrainTile(tilePrefab, tile);
        return newGameObject;
    }
    
    protected GameObject InstantiateNewTerrainTile(GameObject tilePrefab, Tile<TileData> tile)
    {
        GameObject instance = Instantiate(tilePrefab, transform, true);
        instance.transform.position = tile.CartesianPosition;
        instance.name = "Tile - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);
        TileObjects[tile.Index] = instance;
        return instance;
    }

    protected static Vector3 RandomHexRotation()
    {
        

        int degrees = Random.Range(0, 6) * 60;
        
        return new Vector3(0, degrees, 0); 
    }

    protected void AdjustCameraToFit()
    {
        Camera.main.transform.position = new Vector3(HexMap.MapSizeData.center.x, 4, HexMap.MapSizeData.center.z);
        Camera.main.orthographic = true;
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
        Camera.main.orthographicSize = HexMap.MapSizeData.extents.z * 2 * 0.8f;
    }

    public void AddBuiltObject(Vector3Int position, GameObject newBuiltObject)   
    {
        builtObjects.Add(newBuiltObject);
        
        List<GameObject> list;
        
        if (!builtObjectsByPosition.TryGetValue(position, out list))
        {
            list = new List<GameObject>();
            builtObjectsByPosition.Add(position, list);
        }

        list.Add(newBuiltObject);
    }
}
