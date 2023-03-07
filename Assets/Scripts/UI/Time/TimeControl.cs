using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private Button[] _timeButtons = new Button[5];
    [SerializeField] private TextMeshProUGUI timeText;
    private float _timeRecorder = 0;
    private float _seconds = 0;
    private int _minutes = 0;
    private int _hours = 0;
    private int _days = 0;
    private List<float> _monthlyBalance = new List<float>();
    private GameVariableConnector _gameVariableConnector;

    private void Start()
    {
        for (int i = 0; i < _timeButtons.Length; i++)
        {
            _timeButtons[i] = transform.GetChild(i).GetComponent<Button>();
        }

        _gameVariableConnector = GameVariableConnector.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameVariableConnector.IsGamePaused())
        {
            UpdateTimeText();
        }
    }

    private void UpdateTimeText()
    {
        _timeRecorder += Time.deltaTime * 100;

        if (_timeRecorder >= 1)
        {
            _seconds++;
            _timeRecorder = 0;
        }

        if (_seconds >= 60)
        {
            _minutes++;
            _seconds = 0;
        }

        if (_minutes >= 60)
        {
            _hours++;
            _minutes = 0;
        }

        timeText.text = _hours.ToString("00") + ":" + _minutes.ToString("00") + ":" + _seconds.ToString("00");

        if (_hours >= 24)
        {
            _hours = 0;
            _days++;
            NextDaySales();
        }

        if (_days >= 30)
        {
            _days = 0;
            _gameVariableConnector.economyManagerScript.AddMonthlyBalance();
        }
    }

    public void SkipDay()
    {
        _days++;
        _hours = 0;
        _minutes = 0;
        _seconds = 0;
        NextDaySales();
    }

    private void NextDaySales()
    {
        foreach (Product product in CreateJob.completeProducts)
        {
            if (product.Popularity > 0)
            {
                product.Popularity -= (Random.Range(0, 5) + product.Price) * product.Age;

                GameVariableConnector.instance.GetComponent<EconomyManager>().DaySales(product);
            }

            product.Age += 1;
        }
    }
}
