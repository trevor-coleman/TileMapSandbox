using System;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using UnityEngine.Rendering;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;


namespace Navigation
{
    public class AStarSearch 
    {
        
        private Dictionary<Tile<TileData>, Tile<TileData>> cameFrom = new Dictionary<Tile<TileData>, Tile<TileData>>();
        private Dictionary<Tile<TileData>, float> costSoFar = new Dictionary<Tile<TileData>, float>();
        public bool reachedDestination;
        
        public delegate IEnumerable<Tile<TileData>> NeighborFunction(WorldMap worldMap, Tile<TileData> tile);

        public static float Heuristic(Tile<TileData> a, Tile<TileData> b)
        {
            return Vector3Int.Distance(a.Position, b.Position);
        }
        
        public AStarSearch(WorldMap worldMap, Tile<TileData> start, Tile<TileData> goal, NeighborFunction getNeighbors)
        {
            SimplePriorityQueue<Tile<TileData>,float> frontier = new SimplePriorityQueue<Tile<TileData>, float>();
            frontier.Enqueue(start,0f);

            cameFrom[start] = start;
            costSoFar[start] = 0;
            
            

            while (frontier.Count > 0)
            {
                Tile<TileData> current = frontier.Dequeue();

                if (current == goal)
                {
                    reachedDestination = true;
                    break;
                }

                foreach (Tile<TileData> next in getNeighbors(worldMap, current) )
                {
                    float newCost = costSoFar[current] + worldMap.GetCost(current, next);
                    
                    if (costSoFar.ContainsKey(next) && !(newCost < costSoFar[next])) continue;
                    
                    costSoFar[next] = newCost;
                    float priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;                    
                }

            }
            
            Debug.Log("Destination Unreached");
        }
    } 
    
   
}