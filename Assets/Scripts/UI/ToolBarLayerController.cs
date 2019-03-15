using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ToolBarLayerController : MonoBehaviour
    {

        private ToolbarController toolbarController;

        private void Start()
        {
            toolbarController = GetComponentInChildren<ToolbarController>();
        }

        public void Show(ToolbarProperties properties)
        {
            toolbarController.Show(properties);
        }

        public void Hide()
        {
            toolbarController.Hide();    
        }

    }
}