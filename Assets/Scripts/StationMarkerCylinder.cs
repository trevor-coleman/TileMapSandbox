using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StationMarkerCylinder : MonoBehaviour
{
    public enum State
    {
        Default,
        Prohibited
    }

    public State currentState;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material prohibitedMaterial;

    [SerializeField] private GameObject cylinder;
    private MeshRenderer cylinderMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        cylinderMeshRenderer = cylinder.GetComponent<MeshRenderer>();
        currentState = State.Default;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Default:
                SetMaterial(defaultMaterial);
                break;
            case State.Prohibited:
                SetMaterial(prohibitedMaterial);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetMaterial(Material material)
    {
        cylinderMeshRenderer.material = material;
    }
}