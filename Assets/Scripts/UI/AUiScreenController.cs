using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class AUiScreenController<T> : MonoBehaviour
    {

        protected T Properties;

        protected bool IsVisible { get; set; }
        public string ScreenId { get; set; }
                
        public abstract void Show(T properties);

        public abstract void Hide();

    }
   
}