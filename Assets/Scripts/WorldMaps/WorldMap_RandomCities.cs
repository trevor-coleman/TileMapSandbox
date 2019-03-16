using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class WorldMap_RandomCities : WorldMap
{

    [SerializeField] private GameObject cityTilePrefab;
    private GameObject previousHoverTileObject;
    private WorldTile hoverTileComponent;
    [SerializeField] private GameObject suburbTilePrefab;
    [SerializeField] private int numberOfLargeCities;
    [SerializeField] private int numberOfCities;


    // Start is called before the first frame update
    void Start()
    {

        Init();

        AddRandomCitiesToMap();
        AddSuburbs();
        
        if(adjustCameraToFit) AdjustCameraToFit();
    }

    void Update()
    {
        if (!HexMouse.CursorIsOnMap) return;

        MouseTilePosition = HexMouse.TileCoord;

    }
    
    private void AddRandomCitiesToMap()
    {

        for (int i = 0; i < numberOfCities; i++)
        {
            int randomIndex = Random.Range(0, HexMap.TilesByPosition.Count);

            Tile<TileData> tile = HexMap.Tiles[randomIndex];
            
            GameObject tileObject = ReplaceTerrainTile(tile, cityTilePrefab);

            tileObject.transform.rotation = Quaternion.Euler(RandomHexRotation());

            tileObject.name = "City - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);
            tile.Data.Type = TileType.City;
            
            if (i < numberOfLargeCities)
            {
                tile.Data.population = Random.Range(12, 15);          
            }
            else
            {
                tile.Data.population = Random.Range(8, 12);
            }
        }
    }

    private void AddSuburbs()
    {
        foreach (Tile<TileData> tile in HexMap.Tiles)
        {
            if (tile.Data.population > 10)
            {
                Debug.Log(TileObjects[tile.Index].name + " === " + tile.Data + " --- ");
                
                int numberOfSuburbs = tile.Data.population - 10;
                tile.Data.population = 10;

                List<Tile<TileData>> ringTiles = HexMap.GetTiles.AdjacentToTile(tile);
        
                for (int i = 0; i < numberOfSuburbs; i++)
                {
                    Tile<TileData> neighborTile = ringTiles[Random.Range(0, ringTiles.Count)];   
                    if(neighborTile.Data.Populated) continue;
                    GameObject neighborTileObject =  ReplaceTerrainTile(neighborTile, suburbTilePrefab);
                    neighborTileObject.name = "Suburb - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);

                    neighborTile.Data.population = Random.Range(0, 8);
                    neighborTile.Data.Type = TileType.Suburb;

                    ringTiles.Remove(neighborTile);

                }
                
            }    
        }

    }

    // Update is called once per frame
   
}
