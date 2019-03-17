using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentBalancePanel : MonoBehaviour
{

    private TextMeshProUGUI textMeshProUgui;
    [SerializeField] private int bankBalance;
    
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUgui = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void UpdateBalance(int bankBalance)
    {
        this.bankBalance = bankBalance;
        textMeshProUgui.text = "$" + bankBalance.ToString("N0");

    }

    public void ShowInfiniteMoney()
    {
        textMeshProUgui.text = "$infinite";
    }
}
