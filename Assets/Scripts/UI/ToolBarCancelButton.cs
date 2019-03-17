using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolBarCancelButton : MonoBehaviour
    {

        private Button button;
        private ToolManager toolManager;
    
        // Start is called before the first frame update
        void Start()
        {
            toolManager = FindObjectOfType<ToolManager>();
            button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {
            button.interactable = toolManager.ActiveTool != Tool.None;
        }

        public void OnButtonPress()
        {
            toolManager.SetToolToNone();
        }
    }
}
