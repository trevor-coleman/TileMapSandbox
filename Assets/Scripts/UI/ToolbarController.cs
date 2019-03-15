using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wunderwunsch.HexMapLibrary;

namespace UI
{
    public class ToolbarController : AUiScreenController<ToolbarProperties>
    {

        private CanvasGroup canvasGroup;
        [SerializeField] private ToolbarProperties properties;

        private void Start()
        {
            ScreenId = "Toolbar";
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show(ToolbarProperties properties)
        {
            this.properties = properties;
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
    public struct ToolbarProperties 
    {
        public Tool ActiveTool;
    }
}