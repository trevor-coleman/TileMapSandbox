using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Wunderwunsch.HexMapLibrary;

namespace UI
{
    public class ToolbarController : AUiScreenController<ToolbarProperties>
    {
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
                ShowCanvas();
            }
        }

        public override void Hide()
        {
            HideCanvas();
        }
    }

    [Serializable]
    public struct ToolbarProperties 
    {
        public Tool activeTool;
    }
}