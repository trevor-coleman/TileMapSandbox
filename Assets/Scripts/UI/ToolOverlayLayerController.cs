using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace UI
{
    public class ToolOverlayLayerController : MonoBehaviour
    {
        private BuildStationOverlayController buildStationOverlayController;
        private Tool activeTool;

        // Start is called before the first frame update
        void Start()
        {
            buildStationOverlayController = GetComponentInChildren<BuildStationOverlayController>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void UpdateTool<T>(ToolOverLayProperties<T> properties) where T: IToolProperties
        {
            switch (properties.Tool)
            {
                case Tool.BuildStation:
                    buildStationOverlayController.Show(properties as ToolOverLayProperties<BuildStationOverlayProperties>);
                    activeTool = properties.Tool;
                    break;
                case Tool.None:
                    break;
                case Tool.BuildTracks:
                case Tool.Demolish:
                default:
                    break;
                    
            }
        }

        public void CancelTool(object tool)
        {
            switch (tool)
            {
                case Tool.BuildStation:
                    buildStationOverlayController.Hide();
                    break;
                default:
                    break;
            }
        }
    }

    

    [Serializable]
    public class ToolOverLayProperties<T> where T : IToolProperties
    {
        public Tool Tool { get; set; }
        public T Data { get; set; }
    }
    
    public interface IToolProperties
    {
        
    }
}
