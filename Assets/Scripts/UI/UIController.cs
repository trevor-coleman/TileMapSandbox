using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        private ToolBarLayerController toolBarLayerController;
        private ToolOverlayLayerController toolOverlayLayerController;
    
        // Start is called before the first frame update
        void Start()
        {
            toolBarLayerController = GetComponentInChildren<ToolBarLayerController>();
            toolOverlayLayerController = GetComponentInChildren<ToolOverlayLayerController>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateToolOverlay<T>(ToolOverLayProperties<T> properties) where T: IToolProperties
        {
            toolOverlayLayerController.UpdateTool<T>(properties);
        }

        public void CancelTool(Tool tool)
        {
            toolOverlayLayerController.CancelTool(tool);
        }
    }


}


