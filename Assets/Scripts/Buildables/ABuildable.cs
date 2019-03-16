using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

namespace Buildables
{
    public abstract class ABuildable<T> : MonoBehaviour, IBuildable<T> where T: IBuildableData
    {
        public Tile<TileData> Tile { get; set; }
        public WorldMap WorldMap { get; set; }
        public abstract void Build(Tile<TileData> tile, T data);
        public abstract void UpdateData(T data);
    }
}

public interface IBuildable<in T> where T: IBuildableData
{
    Tile<TileData> Tile { get; set;}
    WorldMap WorldMap { get; set; }
    void Build(Tile<TileData> tile, T data);
}

public interface IBuildableData
{
    Tile<TileData> Tile { get;}
    Buildable Buildable { get;}
}