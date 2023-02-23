using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameVariableConnector : MonoBehaviour
{
    public static GameVariableConnector instance = null; // The instance of the SmartObjectManager
    public EconomyManager economyManagerScript;
    [HideInInspector] public GeneralGUIManager generalGUIManagerScript;
    public static float timeScale = 1f;
    public static bool pauseGame = false;

    [SerializeField] private TextAsset _namesJson = null;

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

    private void Start()
    {
        economyManagerScript = GetComponent<EconomyManager>();
        generalGUIManagerScript = GetComponent<GeneralGUIManager>();
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public void ChangeTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
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

    public void PauseGame()
    {
        pauseGame = true;
    }

    public void UnpauseGame()
    {
        pauseGame = false;
    }

    public bool IsGamePaused()
    {
        return pauseGame;
    }
}
