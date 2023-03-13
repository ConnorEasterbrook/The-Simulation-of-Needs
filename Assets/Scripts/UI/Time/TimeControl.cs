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

    private float _chanceOfNewProduct = 0;

    private bool _initialWait = true;

    private void Start()
    {
        for(int i = 0; i < _timeButtons.Length; i++)
        {
            _timeButtons[i] = transform.GetChild(i).GetComponent<Button>();
        }

        _gameVariableConnector = GameVariableConnector.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameVariableConnector.IsGamePaused())
        {
            UpdateTimeText();

            /*if(!_initialWait)
            {
                SpecificTimeChecks();
            }
            else
            {
                if (_seconds >= 1)
                {
                    _initialWait = false;
                }
            }*/
        }
    }

    private void UpdateTimeText()
    {
        if(Time.timeScale == 1)
        {
            _timeRecorder += Time.deltaTime;
        }
        else if(Time.timeScale == 4)
        {
            _timeRecorder += Time.deltaTime * 50;
        }
        else
        {
            _timeRecorder += Time.deltaTime * 500;
        }

        if(_timeRecorder >= 1)
        {
            _seconds++;
            _timeRecorder = 0;
        }

        if(_seconds >= 60)
        {
            _minutes++;
            _seconds = 0;
        }

        if(_minutes >= 60)
        {
            _hours++;
            _minutes = 0;
        }

        timeText.text = _hours.ToString("00") + ":" + _minutes.ToString("00") + ":" + _seconds.ToString("00");

        if(_hours >= 24)
        {
            _hours = 0;
            _days++;
            NextDaySales();
        }

        if(_days >= 30)
        {
            _days = 0;
            _gameVariableConnector.economyManagerScript.AddMonthlyBalance();
        }
    }

    private void SpecificTimeChecks()
    {
        if(_hours > 17 || _hours < 9)
        {
            HidePerformers();
        }
        else
        {
            ShowPerformers();
        }
        if(_hours == 16 && _minutes >= 59)
        {
            HidePerformers();
        }
        else if(_hours == 8 && _minutes >= 59)
        {
            ShowPerformers();
        }

    }

    public void SkipDay()
    {
        if(_hours < 9)
        {
            _hours = 9;
            _minutes = 0;
            _seconds = 0;
        }
        else
        {
            _days++;
            _hours = 9;
            _minutes = 0;
            _seconds = 0;
            NextDaySales();
        }
    }

    private void NextDaySales()
    {
        foreach(Product product in CreateJob.completeProducts)
        {
            if(product.Popularity > 0)
            {
                GameVariableConnector.instance.GetComponent<EconomyManager>().DaySales(product);
                product.Popularity -= (Random.Range(0, 5) + product.Price) * product.Age;
            }

            product.Age += 1;
        }

        _chanceOfNewProduct += Random.Range(0, 20);
        Mathf.Clamp(_chanceOfNewProduct, 0, 100);

        if(_chanceOfNewProduct >= 100)
        {
            _chanceOfNewProduct = 0;
            RandomCompany.instance.CreateRandomProduct();
        }
    }

    private void HidePerformers()
    {
        foreach(BaseCharacterIntelligence performer in BaseCharacterIntelligence.performers)
        {
            if(performer.gameObject.activeSelf)
            {
                performer.gameObject.SetActive(false);
            }
        }
    }

    private void ShowPerformers()
    {
        foreach(BaseCharacterIntelligence performer in BaseCharacterIntelligence.performers)
        {
            if(!performer.gameObject.activeSelf)
            {
                performer.gameObject.SetActive(true);
            }
        }
    }
}
