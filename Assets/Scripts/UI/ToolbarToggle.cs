using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolbarToggle : MonoBehaviour
    {

        [SerializeField] private Tool tool;
        private bool previousState;

        private Toggle toggle;
        private ToolManager toolManager;
    
        // Start is called before the first frame update
        void Start()
        {
            toggle = GetComponent<Toggle>();
            toolManager = FindObjectOfType<ToolManager>();
        }

        // Update is called once per frame
        void Update()
        {
            toggle.isOn = toolManager.ActiveTool == tool;
        }

        public void OnToggleClick()
        {
            toolManager.Toggle(tool);
        }
    }
}
