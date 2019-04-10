using System.Collections.Generic;
using Buildables;
using Navigation;
using UI;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

namespace DefaultNamespace
{
    public class TransitSystem
    {

        public List<Station> Stations;
        private WorldMap worldMap;


        public TransitSystem(WorldMap worldMap)
        {
            this.worldMap = worldMap;
            Stations = new List<Station>();
            Debug.Log("I am a transit system!");
        }

        public void UpdateConnectionsForStation(Station station)
        {
            if (Stations.Count <= 1) return;

            foreach (Station otherStation in Stations)
            {
                if (station == otherStation) continue;

                AStarSearch aStarSearch = new AStarSearch(worldMap, station.Tile, otherStation.Tile, GetNeighborsWithTracks);

                if (!aStarSearch.reachedDestination) continue;

                station.connectedStations.Add(otherStation);

                Debug.Log("Station Added - " + otherStation.Tile.Position);

            }
        }

        public void UpdateAllStationConnections()
        {
            foreach (Station station in Stations)
            {
                UpdateConnectionsForStation(station);
            }
        }

        public static IEnumerable<Tile<TileData>> GetNeighborsWithTracks(WorldMap worldMap, Tile<TileData> tile)
        {
            IEnumerable<Tile<TileData>> allNeighbors = worldMap.GetNeighbors(tile);

            foreach (Tile<TileData> neighbor in allNeighbors)
            {
                if (neighbor.Data.HasTracks)
                {
                    yield return neighbor;
                }
            }
        }
    }
}