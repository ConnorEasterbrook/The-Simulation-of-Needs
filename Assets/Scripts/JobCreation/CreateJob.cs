using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateJob : MonoBehaviour
{
    public static CreateJob instance = null; // The instance of the SmartObjectManager

    public List<Slider> sliders = new List<Slider>();
    private List<Slider> _availableSliders = new List<Slider>();
    private List<Slider> _activeSliders = new List<Slider>();
    private string _taskName;

    // Store all task names created to make sure there are no duplicates
    private Dictionary<string, string> _taskNames = new Dictionary<string, string>();

    public bool isCreatingTask;

    private void Awake()
    {
        // If an instance of the SmartObjectManager already exists, destroy this instance
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _availableSliders = sliders;
    }

    public bool CheckForAvailableSlider()
    {
        for (int i = 0; i < _availableSliders.Count; i++)
        {
            if (!_availableSliders[i].gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckForActiveSlider()
    {
        for (int i = 0; i < _activeSliders.Count; i++)
        {
            if (_activeSliders[i].value < 100)
            {
                return true;
            }
        }

        return false;
    }

    public void WorkOnTask(float percentageAmount, float increaseSpeed)
    {
        int randomTask = Random.Range(0, _activeSliders.Count);
        IncreaseProgress(percentageAmount, increaseSpeed, _activeSliders[randomTask]);
    }

    public void IncreaseProgress(float percentageAmount, float increaseSpeed, Slider slider)
    {
        slider.GetComponent<ProgressOnBar>().IncreaseProgress(percentageAmount, increaseSpeed);
    }

    public void SetTaskName(TMP_InputField inputField)
    {
        _taskName = inputField.text;
    }

    public void CreateTask()
    {
        for (int i = 0; i < _availableSliders.Count; i++)
        {
            if (!_availableSliders[i].gameObject.activeInHierarchy && !_taskNames.ContainsKey(_taskName))
            {
                _availableSliders[i].gameObject.SetActive(true);
                _availableSliders[i].GetComponentInChildren<TextMeshProUGUI>().text = _taskName;
                _taskNames.Add(_taskName, _taskName);
                _activeSliders.Add(_availableSliders[i]);
                _availableSliders.RemoveAt(i);
                break;
            }
            else if (_taskNames.ContainsKey(_taskName))
            {
                Debug.Log("Task name already exists");
                break;
            }
        }
    }
}
