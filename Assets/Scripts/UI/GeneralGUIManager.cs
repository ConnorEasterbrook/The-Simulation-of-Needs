using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _GUIPanel;
    [SerializeField] private GameObject _buildingModePanel;

    private GridBuildCore _gridBuildCore;

    private void Start()
    {
        _gridBuildCore = GetComponent<GridBuildCore>();
    }

    public void ChangeBuildingMode()
    {
        bool isBuilding = GridBuildCore.isBuilding;

        if (isBuilding)
        {
            _buildingModePanel.SetActive(false);
            _GUIPanel.SetActive(true);
            GridBuildCore.isBuilding = false;
        }
        else
        {
            _buildingModePanel.SetActive(true);
            _GUIPanel.SetActive(false);
            GridBuildCore.isBuilding = true;
        }
    }

    public void SetBuildingObject(GameObject buildingObject)
    {
        GridBuildCore gridBuildCore = FindObjectOfType<GridBuildCore>();
        gridBuildCore.SetBuildingObject(buildingObject);
    }
}
