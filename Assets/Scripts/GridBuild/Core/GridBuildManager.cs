using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildManager : MonoBehaviour
{
    public static bool isBuilding = false;
    public static GridBuildCore _gridBuildCore = new GridBuildCore();

    public GameObject objectPrefab;
    public GameObject previewObject;
    public GameObject plane;
    public Plane gridPlane;
    public float tileSize = 1f;

    private ObjectTypes _objectType;
    private WallBuilder _wallBuilder = new WallBuilder();
    private FloorBuilder _floorBuilder = new FloorBuilder();
    private RoomObject _roomBuilder;
    private GroundFurnitureBuild _groundFurnitureBuilder = new GroundFurnitureBuild();
    private DeskFurnitureBuild _deskFurnitureBuilder = new DeskFurnitureBuild();

    // Start is called before the first frame update
    void Start()
    {
        gridPlane = new Plane(plane.transform.up, plane.transform.position);
        _gridBuildCore.PrepVariables(objectPrefab, previewObject, plane, gridPlane, tileSize);
        _objectType = objectPrefab.GetComponent<BuildableObject>().objectType;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            switch (_objectType)
            {
                case ObjectTypes.Wall:
                    _wallBuilder.BuildObject();
                    break;

                case ObjectTypes.Floor:
                    break;

                case ObjectTypes.Room:
                    break;

                case ObjectTypes.Ground_Furniture:
                    _groundFurnitureBuilder.BuildObject();
                    break;

                case ObjectTypes.Desk_Furniture:
                    break;

                default:
                    break;
            }
        }

        // Check if raycast hits UI element
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            _gridBuildCore.UpdateValidity(false);
        }
        else
        {
            _gridBuildCore.UpdateValidity(true);
        }
    }

    public void SetPreviewObject(GameObject _previewObject)
    {
        previewObject = _previewObject;
    }

    /// <summary>
    /// Sets the building object to be placed
    /// </summary>
    public void SetBuildingObject(GameObject buildingObject)
    {
        _gridBuildCore.ChangeObject(buildingObject, previewObject);
        _objectType = buildingObject.GetComponent<BuildableObject>().objectType;
    }
}

public enum ObjectTypes
{
    Wall,
    Floor,
    Room,

    Ground_Furniture,
    Desk_Furniture
}
