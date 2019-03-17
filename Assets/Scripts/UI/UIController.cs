using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        private ToolBarLayerController toolBarLayerController;
        private ToolOverlayLayerController toolOverlayLayerController;
        private HudLayerController hudLayerController;
            
    
        // Start is called before the first frame update
        void Start()
        {
            toolBarLayerController = GetComponentInChildren<ToolBarLayerController>();
            toolOverlayLayerController = GetComponentInChildren<ToolOverlayLayerController>();
            hudLayerController = GetComponentInChildren<HudLayerController>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateToolOverlay<T>(ToolOverLayProperties<T> properties) where T: IToolProperties
        {
            toolOverlayLayerController.UpdateTool<T>(properties);
        }

        public void UpdateHud<T>(HudDisplayUpdate<T> update) where T: IHudDisplayData
        {
            hudLayerController.Show(update);
        }

        public void CancelTool(Tool tool)
        {
            toolOverlayLayerController.CancelTool(tool);
        }
    }


}


