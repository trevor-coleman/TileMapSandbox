using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTile : WorldTile, IPopulated
{

    [SerializeField] private int population;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPopulation(int population)
    {
        this.population = population;
    }

    public int GetPopulation()
    {
        return population;
    }
}


