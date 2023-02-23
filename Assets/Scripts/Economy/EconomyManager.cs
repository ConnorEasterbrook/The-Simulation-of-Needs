using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager
{
    private TextMeshProUGUI _balanceText;

    private float _balance = 1000;

    private List<float> _monthlyBalance = new List<float>();
    private List<float> _monthlyProfit = new List<float>();
    private List<float> _monthlyIncome = new List<float>();
    private List<float> _monthlyExpenses = new List<float>();


    public EconomyManager(TextMeshProUGUI balanceText)
    {
        _balanceText = balanceText;
    }

    public void SetStartingBalance(float balance)
    {
        _balance = balance;
    }
    
    public void UpdateBalance()
    {
        _balanceText.text = "£" + _balance.ToString("0.00");
    }

    public void AddToBalance(float amount)
    {
        _balance += amount;
    }

    public void SubtractFromBalance(float amount)
    {
        _balance -= amount;
    }

    public void AddMonthlyBalance()
    {
        _monthlyBalance.Add(_balance);
    }
}
