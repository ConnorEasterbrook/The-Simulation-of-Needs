using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariableConnector : MonoBehaviour
{
    public static GameVariableConnector instance = null; // The instance of the SmartObjectManager
    public ProgressOnBar _progressOnBarScript;
    public GeneralGUIManager _generalGUIManagerScript;

    public ProgressOnBar GetProgressOnBarScript()
    {
        return _progressOnBarScript;
    }

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

    public void IncreaseProgress(float percentageAmount, float increaseSpeed)
    {
        _progressOnBarScript.IncreaseProgress(percentageAmount, increaseSpeed);
    }
}
