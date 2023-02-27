using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameVariableConnector : MonoBehaviour
{
    public static GameVariableConnector instance = null; // The instance of the SmartObjectManager
    [HideInInspector] public EconomyManager economyManagerScript;
    [HideInInspector] public GeneralGUIManager generalGUIManagerScript;
    public static float timeScale = 1f;
    public static bool pauseGame = true;

    public GameObject wallParent;
    public GameObject floorParent;
    public GameObject furnitureParent;

    [SerializeField] private TextAsset _namesJson = null;
    [SerializeField] private TextAsset _traitsJson = null;

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

    public TextAsset GetTraitsJson()
    {
        if (_traitsJson == null)
        {
            Debug.LogError("No traits.json file found!");
            return null;
        }

        return _traitsJson;
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
