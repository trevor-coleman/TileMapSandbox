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
        private CanvasGroup canvasGroup;

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
                IsVisible = true;
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public override void Hide()
        {
            IsVisible = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    [Serializable]
    public struct BuildStationOverlayProperties : IToolProperties
    {
        public int population;
    }
}