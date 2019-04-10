using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TransitSystem transitSystem;
    public WorldMap worldMap;
    public MoneyManager moneyManager;
    
    // Start is called before the first frame update
    void Start()
    {
        worldMap = FindObjectOfType<WorldMap>();        
        transitSystem = new TransitSystem(worldMap);
        moneyManager = FindObjectOfType<MoneyManager>();
    }

}
