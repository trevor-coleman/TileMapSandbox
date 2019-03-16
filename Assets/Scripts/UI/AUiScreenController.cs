using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class AUiScreenController<T> : MonoBehaviour
    {

        protected T Properties;
        protected CanvasGroup canvasGroup;

        protected bool IsVisible { get; set; }
        public string ScreenId { get; set; }
                
        public abstract void Show(T properties);

        public abstract void Hide();
        
        protected virtual void ShowCanvas()
        {
            IsVisible = true;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        protected virtual void HideCanvas()
        {
            IsVisible = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

    }
   
}