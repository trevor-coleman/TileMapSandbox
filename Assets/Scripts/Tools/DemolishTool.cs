using System.Collections;
using System.Collections.Generic;
using Buildables;
using UnityEngine;
using UnityEngine.EventSystems;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

namespace Tools
{
    public class DemolishTool : MonoBehaviour
    {
    
        private WorldMap worldMap;
        private ToolManager toolManager;
        private Builder builder;
        
        private bool cleanedUp;

        // Start is called before the first frame update
        private void Start()
        {
            worldMap = FindObjectOfType<WorldMap>();
            builder = FindObjectOfType<Builder>();
            toolManager = FindObjectOfType<ToolManager>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (toolManager.ActiveTool != Tool.Demolish)
            {
                if (cleanedUp) return;

                CleanUp();
                return;
            }
        
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(worldMap.MouseTile);
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Over object");
                    PointerEventData pointer = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };

                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointer, raycastResults);

                    if(raycastResults.Count > 0)
                    {
                        foreach(RaycastResult result in raycastResults)
                        {  
                            Debug.Log(result.gameObject.name,result.gameObject);
                        }
                    }
                }
                else
                {
                    DemolishCommand<TrackData> demolishStation=
                        new DemolishCommand<TrackData>(builder, new TrackData(worldMap.MouseTile));
                    builder.ExecuteCommand(demolishStation);
                }
            }
        }

        private void CleanUp()
        {
        }
    }
}