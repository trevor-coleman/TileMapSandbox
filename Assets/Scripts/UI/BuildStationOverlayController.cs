using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class
        BuildStationOverlayController : AUiScreenController<ToolOverLayProperties<BuildStationOverlayProperties>>
    {
        private ToolOverLayProperties<BuildStationOverlayProperties> properties;
        private TextMeshProUGUI populationDisplay;


        // Start is called before the first frame update
        private void Start()
        {
            populationDisplay = GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log(populationDisplay);
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public override void Show(ToolOverLayProperties<BuildStationOverlayProperties> properties)
        {
            this.properties = properties;
            
            populationDisplay.text = properties.Data.population.ToString();
            
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
    public struct BuildStationOverlayProperties : IToolProperties
    {
        public int population;
    }
}