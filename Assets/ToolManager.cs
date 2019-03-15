using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tool
{
    None,
    BuildStation,
    BuildTracks,
    Demolish,
    
}

public class ToolManager : MonoBehaviour
{
    [SerializeField] private Tool activeTool;
    public Tool ActiveTool => activeTool;
       
    private void DeselectTool(Tool tool)
    {
        if (activeTool == tool)
        {
            activeTool = Tool.None;
        }
    }

    public void SetToolToNone()
    {
        activeTool = Tool.None;
    }
    
    public void SetActiveTool(Tool newTool)
    {
        activeTool = newTool;
    }

    public void Toggle(Tool tool)
    {
        if (activeTool == tool)
        {
            DeselectTool(activeTool);
        }
        else
        {
            SetActiveTool(tool);
        }
    }
}

