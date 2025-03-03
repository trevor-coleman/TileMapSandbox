﻿using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolBarButton : MonoBehaviour
    {

        private Button button;
        private ToolManager toolManager;
        [SerializeField] private Tool tool;
    
        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            toolManager = FindObjectOfType<ToolManager>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
