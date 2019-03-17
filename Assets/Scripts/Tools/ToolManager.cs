using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public enum Tool
    {
        None,
        BuildStation,
        BuildTracks,
        Demolish,
    }

    public enum DemolishType
    {
        All,
        Tracks,
        Stations
    }

    public class ToolManager : MonoBehaviour
    {

        [SerializeField] public DemolishType ActiveDemolishType;

        [SerializeField] public Tool ActiveTool; 

        private void DeselectTool(Tool tool)
        {
            if (ActiveTool == tool)
            {

                ActiveTool = Tool.None;
            }
        }

        public void SetToolToNone()
        {
            ActiveTool = Tool.None;
        }

        public void SetActiveTool(Tool newTool)
        {
            if (newTool == Tool.Demolish)
            {
                ActiveDemolishType = DemolishType.All;
            }
            ActiveTool = newTool;
        }

        public void Toggle(Tool tool)
        {
            if (ActiveTool == tool)
            {
                DeselectTool(ActiveTool);
            }
            else
            {
                SetActiveTool(tool);
            }
        }

      
        public void ToggleDemolishType(DemolishType demolishType)
        {
        
            if (ActiveDemolishType == demolishType)
            {
                DeselectDemolishType();
            }
            else
            {
                SetActiveDemolishType(demolishType);
            }
        }

        private void SetActiveDemolishType(DemolishType demolishType)
        {
            ActiveDemolishType = demolishType;
            Debug.Log("Selecting - " + ActiveDemolishType);
        }

        private void DeselectDemolishType()
        {
            ActiveDemolishType = DemolishType.All;
            
        }
    }
}