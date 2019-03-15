using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TerrainTileSet", order = 1)]
public class TerrainTileSet : ScriptableObject
{
    
    public GameObject clayPrefab;
    public GameObject coalPrefab;
    public GameObject desertPrefab;
    public GameObject diamondPrefab;
    public GameObject goldPrefab;
    public GameObject ironPrefab;
    public GameObject lakePrefab;
    public GameObject pasturePrefab;
    public GameObject riverPrefab;
    public GameObject stonePrefab;
    public GameObject wastelandPrefab;
    public GameObject waterPrefab;
  
}

