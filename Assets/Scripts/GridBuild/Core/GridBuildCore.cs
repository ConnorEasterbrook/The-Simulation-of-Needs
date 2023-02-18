using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildCore : MonoBehaviour
{
    public static bool isBuilding = false;
    public GameObject objectPrefab;
    public GameObject plane;
    public Plane gridPlane;
    public float tileSize = 1f;
    public Vector3 initialObjectScale;

    private ObjectTypes _objectType;
    private WallBuilder _wallBuilder;
    private FloorBuilder _floorBuilder;

    [HideInInspector] public GameObject _previewObject;
    [HideInInspector] public Vector3 _startPoint;
    [HideInInspector] public Vector3 _endPoint;
    [HideInInspector] public Vector3 _objectPosition;
    [HideInInspector] public  Vector3 _initialObjectScale;

    [HideInInspector] public  Vector3 _direction;
    [HideInInspector] public  float _length;

    // Start is called before the first frame update
    void Start()
    {
        gridPlane = new Plane(plane.transform.up, plane.transform.position);
        initialObjectScale = objectPrefab.transform.localScale;

        _wallBuilder = new WallBuilder();
        // _floorBuilder = new FloorBuilder(this);

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
                    break;

                case ObjectTypes.Desk_Furniture:
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Gets the mouse position on the grid plane
    /// </summary>
    public Vector3 GetMouseWorldPositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (gridPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Snaps the given point to the grid
    /// </summary>
    public Vector3 SnapToGrid(Vector3 hitPoint)
    {
        float x = Mathf.Round(hitPoint.x / tileSize) * tileSize;
        float y = hitPoint.y + (_initialObjectScale.y * 0.5f);
        float z = Mathf.Round(hitPoint.z / tileSize) * tileSize;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Sets the building object to be placed
    /// </summary>
    public void SetBuildingObject(GameObject buildingObject)
    {
        objectPrefab = buildingObject;
        initialObjectScale = objectPrefab.transform.localScale;
        _objectType = objectPrefab.GetComponent<BuildableObject>().objectType;
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
