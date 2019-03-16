using System;
using UnityEngine;

namespace UI
{

    public enum HudDisplay
    {
        Finance
    }
    
    public class HudLayerController : MonoBehaviour
    {

        private FinanceDisplayController financeDisplayController;

        private void Start()
        {
            financeDisplayController = GetComponentInChildren<FinanceDisplayController>();
        }

        public void Show<T>(HudDisplayUpdate<T> update) where T: IHudDisplayData
        {
            switch (update.HudDisplay)
            {
                case HudDisplay.Finance:
                    Debug.Log("HudLayer " + update.Data);
                    financeDisplayController.Show(update.Data as FinanceDisplayData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void Hide()
        {
            financeDisplayController.Hide();    
        }

    }

    public interface IHudDisplayUpdate<in T> where T : IHudDisplayData
    {
        HudDisplay HudDisplay { get; set; }
        IHudDisplayData Data { get; }
    }
    
    public class HudDisplayUpdate<T> : IHudDisplayUpdate<T> where T: IHudDisplayData
    {
        public HudDisplay HudDisplay { get; set; }
        public IHudDisplayData Data { get; }
        
        public HudDisplayUpdate(HudDisplay hudDisplay, T data)
        {
            this.HudDisplay = hudDisplay;
            this.Data = data;
        }
        
    }

    public interface IHudDisplayData
    {
    }
}


