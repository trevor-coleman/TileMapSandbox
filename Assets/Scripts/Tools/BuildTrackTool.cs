using System.Collections;
using System.Collections.Generic;
using Buildables;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

namespace Tools
{
    public class BuildTrackTool : MonoBehaviour
    {
    
        private WorldMap worldMap;
        private ToolManager toolManager;
        private Builder builder;

        [SerializeField] private GameObject trackPrefab;
        private bool cleanedUp;

        // Start is called before the first frame update
        private void Start()
        {
            worldMap = FindObjectOfType<WorldMap>();
            builder = FindObjectOfType<Builder>();
            toolManager = FindObjectOfType<ToolManager>();
        }

        // Update is called once per frame
        private void Update()
        {
        
            if (toolManager.ActiveTool != Tool.BuildTracks)
            {
                if (cleanedUp) return;

                CleanUp();
                return;
            }
        
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(worldMap.MouseTile);
                Debug.Log(worldMap.MouseTile.Data);
                Debug.Log(worldMap.MouseTile.Data.HasTracks);
            
            
                BuildCommand<TrackData> buildStation =
                    new BuildCommand<TrackData>(builder, new TrackData(worldMap.MouseTile));
                builder.ExecuteCommand(buildStation);
            }
        }

        private void CleanUp()
        {
        }
    }

    public class TrackData : IBuildableData
    {
        public Tile<TileData> Tile { get; }
        public Buildable Buildable { get; }
        public int Price { get; }

        public TrackData(Tile<TileData> tile)
        {
            this.Tile = tile;
            this.Buildable = Buildable.Track;
            this.Price = 1000;
        }
    }
}