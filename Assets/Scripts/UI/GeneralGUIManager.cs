using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGUIManager : MonoBehaviour
{
    public bool stopMovement = false;

    [SerializeField] private GameObject _GUIPanel;
    [SerializeField] private GameObject _buildingModePanel;
    [SerializeField] private GameObject _taskCreationPanel;

    private GridBuildManager _gridBuildManager;

    private void Start()
    {
        _gridBuildManager = GetComponent<GridBuildManager>();
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

    public void ChangeTaskCreationMode()
    {
        bool isCreatingTask = CreateJob.instance.isCreatingTask;

        if (isCreatingTask)
        {
            stopMovement = false;
            _taskCreationPanel.SetActive(false);
            _GUIPanel.SetActive(true);
            CreateJob.instance.isCreatingTask = false;
        }
        else
        {
            stopMovement = true;
            _taskCreationPanel.SetActive(true);
            _GUIPanel.SetActive(false);
            CreateJob.instance.isCreatingTask = true;
        }
    }

    public void SetPreviewObject(GameObject previewObject)
    {
        _gridBuildManager.SetPreviewObject(previewObject);
    }

    public void SetBuildingObject(GameObject buildingObject)
    {
        buildingObject.SetActive(true);
        _gridBuildManager.SetBuildingObject(buildingObject);
    }
}
