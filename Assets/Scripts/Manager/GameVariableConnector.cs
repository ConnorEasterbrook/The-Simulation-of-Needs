using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariableConnector : MonoBehaviour
{
    public static GameVariableConnector instance = null; // The instance of the SmartObjectManager
    public ProgressOnBar progressOnBarScript;
    public GeneralGUIManager generalGUIManagerScript;

    [SerializeField] private TextAsset _namesJson = null;

    public ProgressOnBar GetProgressOnBarScript()
    {
        return progressOnBarScript;
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

    public TextAsset GetNamesJson()
    {
        if (_namesJson == null)
        {
            Debug.LogError("No names.json file found!");
            return null;
        }

        return _namesJson;
    }

    public void IncreaseProgress(float percentageAmount, float increaseSpeed)
    {
        progressOnBarScript.IncreaseProgress(percentageAmount, increaseSpeed);
    }
}
