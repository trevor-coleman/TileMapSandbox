using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    private int bankBalance;
    public int BankBalance => bankBalance; 
    [SerializeField] private int startingBankBalance;

    // Start is called before the first frame update
    void Start()
    {
        bankBalance = startingBankBalance;
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
        if (CanSpend(amount))
        {
            bankBalance -= amount;
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
    }
}
