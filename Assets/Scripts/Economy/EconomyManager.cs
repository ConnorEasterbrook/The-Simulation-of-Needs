using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private TextMeshProUGUI _buildBalanceText;

    private float _balance = 1000;

    private List<float> _monthlyBalance = new List<float>();
    private List<float> _monthlyProfit = new List<float>();
    private List<float> _monthlyIncome = new List<float>();
    private List<float> _monthlyExpenses = new List<float>();

    public void SetStartingBalance(float balance)
    {
        _balance = balance;
    }
    
    public void UpdateBalance()
    {
        _balanceText.text = "£" + _balance.ToString("0.00");
        _buildBalanceText.text = "£" + _balance.ToString("0.00");
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
        GeneralGUIManager generalGUIManagerScript = GameVariableConnector.instance.generalGUIManagerScript;
        AutonomousIntelligence[] performers = generalGUIManagerScript.GetPerformers();

        for (int i = 0; i < performers.Length; i++)
        {
            _balance -= performers[i].characterSkillsScript.GetSalary();
        }

        _monthlyBalance.Add(_balance);
        Debug.Log("Monthly balance: " + _balance);
    }
}
