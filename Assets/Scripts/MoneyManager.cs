using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    private int bankBalance;
    public int BankBalance => bankBalance; 
    [SerializeField] private int startingBankBalance;
    private UIController uiController;
    [SerializeField] private bool infiniteMoney;

    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        bankBalance = startingBankBalance;
        UpdateHUD();

    }

    private void UpdateHUD()
    {

        HudDisplayUpdate<FinanceDisplayData> update = new HudDisplayUpdate<FinanceDisplayData>(
            HudDisplay.Finance, 
            new FinanceDisplayData(
                bankBalance,
                infiniteMoney));

        Debug.Log("moneyManager: " + update.Data);
        uiController.UpdateHud(update);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool CanSpend(int amount)
    {
        return bankBalance > amount;
    }

    public bool Spend(int amount)
    {
        if (infiniteMoney) return true;
        
        if (CanSpend(amount))
        {
            bankBalance -= amount;
            UpdateHUD();
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void Deposit(int amount)
    {
        bankBalance += amount;
        UpdateHUD();
    }
}
