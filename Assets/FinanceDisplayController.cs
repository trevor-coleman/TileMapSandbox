using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class FinanceDisplayController : AUiScreenController<FinanceDisplayData>
{
    private FinanceDisplayData data;
    private CurrentBalancePanel currentBalancePanel;
    
    private void Start()
    {
        IsVisible = true;
        ScreenId = "FinanceDisplay";
        currentBalancePanel = GetComponentInChildren<CurrentBalancePanel>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Hide()
    {
        HideCanvas();
    }

    public override void Show(FinanceDisplayData data)
    {
        this.data = data;
        Debug.Log("FinanceDisplay " + data );
        currentBalancePanel.UpdateBalance(data.BankBalance);
        ShowCanvas();
    }
}

public class FinanceDisplayData: IHudDisplayData
{
    public int BankBalance { get; }

    public FinanceDisplayData(int bankBalance)
    {
        this.BankBalance = bankBalance;
    }
}
