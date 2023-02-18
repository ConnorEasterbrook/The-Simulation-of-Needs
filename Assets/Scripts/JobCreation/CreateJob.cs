using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateJob : MonoBehaviour
{
    public static CreateJob instance = null; // The instance of the SmartObjectManager

    public List<Slider> sliders = new List<Slider>();
    private List<Slider> _availableSliders = new List<Slider>();
    private List<Slider> _activeSliders = new List<Slider>();

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

    public void CreateTask()
    {
        for (int i = 0; i < _availableSliders.Count; i++)
        {
            if (!_availableSliders[i].gameObject.activeInHierarchy)
            {
                _availableSliders[i].gameObject.SetActive(true);
                _activeSliders.Add(_availableSliders[i]);
                _availableSliders.RemoveAt(i);
                break;
            }
        }
    }
}
