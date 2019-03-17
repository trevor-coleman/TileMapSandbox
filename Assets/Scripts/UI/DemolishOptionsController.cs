using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Wunderwunsch.HexMapLibrary;

public class DemolishOptionsController : AUiScreenController<DemolishOptionsProperties>
{
    [SerializeField] private DemolishOptionsProperties properties;
    [SerializeField] private GameObject[] toggleGameObjects;
    private ToolManager toolManager;

    private List<DemolishOptionsToggle> toggles;
    private void Start()
    {
        ScreenId = "DemolishOptions";
        canvasGroup = GetComponent<CanvasGroup>();
        toggles = new List<DemolishOptionsToggle>();
        foreach (GameObject toggleGameObject in toggleGameObjects)
        {
            toggles.Add(toggleGameObject.GetComponent<DemolishOptionsToggle>());
        }

        toolManager = FindObjectOfType<ToolManager>();



    }

    private void Update()
    {
        if (toolManager.ActiveTool != Tool.Demolish)
        {
            HideCanvas();
            return;
        }
        else
        {
            ShowCanvas();
        }

    }

    public override void Show(DemolishOptionsProperties properties)
    {
        this.properties = properties;
        if (!IsVisible)
        {
            ShowCanvas();
        }
    }

    public override void Hide()
    {
        HideCanvas();
    }

}

[Serializable]
public class DemolishOptionsProperties
{
    public DemolishType DemolishType{ get; }

    public DemolishOptionsProperties(DemolishType demolishType)
    {
        this.DemolishType = demolishType;
    }

}
