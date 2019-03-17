using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class DemolishOptionsToggle : MonoBehaviour
{
        public DemolishType DemolishType;
        private bool previousState;

        private Toggle toggle;
        private ToolManager toolManager;
    
        // Start is called before the first frame update
        void Start()
        {
            toggle = GetComponent<Toggle>();
            toolManager = FindObjectOfType<ToolManager>();
            toggle.isOn = false;
        }

        // Update is called once per frame
        void Update()
        {
            toggle.isOn = toolManager.ActiveDemolishType == DemolishType;
        }

        public void OnToggleClick()
        {
            toolManager.ToggleDemolishType(DemolishType);
        }

        public void SetIsOn(bool newState)
        {
            toggle.isOn = newState;
        }
    }

