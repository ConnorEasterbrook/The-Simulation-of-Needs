using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralGUIManager : MonoBehaviour
{
    [Header("General")]
    public bool stopMovement = false;

    [Header("Panels")]
    [SerializeField] private GameObject _GUIPanel;
    [SerializeField] private GameObject _buildingModePanel;
    [SerializeField] private GameObject _taskCreationPanel;

    private GridBuildManager _gridBuildManager;

    [Header("Performer")]
    [SerializeField] private RenderTexture _performerRT;
    [SerializeField] private GameObject _playerPerformer;
    private GameObject _performer;
    private PerformerCam _performerCam;

    [Header("Performer Details")]
    private CharacterNeedsUI _characterNeedsUI;
    [SerializeField] private Transform _performerDetailsPanelParent;
    [SerializeField] private GameObject _performerDetailsPanel;


    private void Awake()
    {
        _gridBuildManager = GetComponent<GridBuildManager>();

        _performer = _playerPerformer;
        _performerCam = new PerformerCam(_performer, _performerRT);

        _characterNeedsUI = new CharacterNeedsUI(_performerDetailsPanel, _performerDetailsPanelParent);
        _characterNeedsUI.GetInitialPerformers();
    }

    private void Update()
    {
        if (!stopMovement)
        {
            DetectPerformer();
        }

        _characterNeedsUI.PopulatePerformerDetails();
    }

    private void DetectPerformer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.gameObject.GetComponent<NavMeshAgent>() != null)
            {
                _performer = hit.collider.gameObject;
                _performerCam.ChangeCamera(_performer);
            }
            else
            {
                _performer = _playerPerformer;
                _performerCam.ChangeCamera(_performer);
            }
        }

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
