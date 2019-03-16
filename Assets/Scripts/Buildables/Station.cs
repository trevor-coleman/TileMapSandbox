using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;
using Wunderwunsch.HexMapLibrary.Generic;

namespace Buildables
{
    public class Station : ABuildable<StationData>
    {
        [SerializeField]private int price;
        private TextMeshPro textMeshPro;
        [SerializeField] private int catchment;
        [SerializeField] private int catchmentRadius;
        private WorldMap worldMap;
        private List<Tile<TileData>> catchmentTiles;
        
        

        private void Start()
        {
            worldMap = FindObjectOfType<WorldMap>();
            Debug.Log("WorldMap - " + worldMap);
            textMeshPro = GetComponentInChildren<TextMeshPro>();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateCatchment();
            textMeshPro.text = catchment.ToString();
        }
        
        public override void Build(Tile<TileData> tile, StationData data)
        {
            this.Tile = tile;
            catchmentRadius = data.CatchmentRadius;
            this.worldMap = worldMap;
            UpdateCatchment();
        }

        private void UpdateCatchment()
        {
            if (!worldMap) {return;}
            catchmentTiles = worldMap.HexMap.GetTiles.Disc(Tile, catchmentRadius, true);
            catchment = CalculateCatchmentPopulation();
        }

        private int CalculateCatchmentPopulation()
        {
            int totalPopulation = 0;
            
            foreach (Tile<TileData> tile in catchmentTiles)
            {
                totalPopulation += tile.Data.population;
            }

            return totalPopulation;
        }

        public override void UpdateData(StationData data)
        {
            Tile = data.Tile;
            catchmentRadius = data.CatchmentRadius;
        }

        

    }

    public class StationData : IBuildableData
    {
        public Tile<TileData> Tile { get; }
        public Buildable Buildable { get; }
        public int CatchmentRadius { get; }

        public int Price => CatchmentRadius * 10000;
        
        public StationData(Tile<TileData>tile, int catchmentRadius)
        {
            this.Buildable = Buildable.Station; 
            CatchmentRadius = catchmentRadius;
            this.Tile = tile;
        }

        
    }
}