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
    

    // Start is called before the first frame update
    void Start()
    {

        Init();

        AddRandomCitiesToMap(10);
        AddSuburbs();
        
        if(adjustCameraToFit) AdjustCameraToFit();
    }

    void Update()
    {
        if (!hexMouse.CursorIsOnMap) return;

        mouseTilePosition = hexMouse.TileCoord;

    }
    
    private void AddRandomCitiesToMap(int numberOfCities)
    {

        for (int i = 0; i < numberOfCities; i++)
        {
            int randomIndex = Random.Range(0, hexMap.TilesByPosition.Count);

            Tile<int> tile = hexMap.Tiles[randomIndex];
            
            GameObject tileObject = ReplaceTerrainTile(tile, cityTilePrefab);
            
            

            tileObject.transform.rotation = Quaternion.Euler(RandomHexRotation());

            tileObject.name = "City - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);
            
            tile.Data = Random.Range(2, 15);

            tileObject.GetComponent<CityTile>().SetPopulation(tile.Data);

        }
    }

    private void AddSuburbs()
    {
        foreach (Tile<int> tile in hexMap.Tiles)
        {
            if (tile.Data > 10)
            {
                Debug.Log(tileObjects[tile.Index].name + " === " + tile.Data + " --- ");
                
                int numberOfSuburbs = tile.Data - 10;
                tile.Data = 10;

                List<Tile<int>> ringTiles = hexMap.GetTiles.AdjacentToTile(tile);
        
                for (int i = 0; i < numberOfSuburbs; i++)
                {
                    
                    
                    Tile<int> neighborTile = ringTiles[Random.Range(0, ringTiles.Count)];
                    
                    if(tileObjects[neighborTile.Index].GetComponent<WorldTile>() is IPopulated) continue;

                    GameObject neighborTileObject =  ReplaceTerrainTile(neighborTile, suburbTilePrefab);
                    
                    neighborTileObject.name = "Suburb - " + HexConverter.TileCoordToOffsetTileCoord(tile.Position);

                    ringTiles.Remove(neighborTile);

                }
                
            }    
        }

    }

    // Update is called once per frame
   
}
