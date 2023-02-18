using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _GUIPanel;
    [SerializeField] private GameObject _buildingModePanel;

    private GridBuildManager _gridBuildCore;

    private void Start()
    {
        _gridBuildCore = GetComponent<GridBuildManager>();
    }

    public void ChangeBuildingMode()
    {
        bool isBuilding = GridBuildManager.isBuilding;

        if (isBuilding)
        {
            _buildingModePanel.SetActive(false);
            _GUIPanel.SetActive(true);
            GridBuildManager.isBuilding = false;
        }
        else
        {
            _buildingModePanel.SetActive(true);
            _GUIPanel.SetActive(false);
            GridBuildManager.isBuilding = true;
        }
    }

    public void SetBuildingObject(GameObject buildingObject)
    {
        GridBuildManager gridBuildCore = FindObjectOfType<GridBuildManager>();
        gridBuildCore.SetBuildingObject(buildingObject);
    }
}
