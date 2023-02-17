using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildCore : MonoBehaviour
{
    public static bool isBuilding = true;
    public GameObject objectPrefab;
    public GameObject plane;
    public Plane gridPlane;
    public float tileSize = 1f;
    public Vector3 initialObjectScale;

    private ObjectTypes _objectType;
    private WallBuilder _wallBuilder;
    private FloorBuilder _floorBuilder;

    // Start is called before the first frame update
    void Start()
    {
        gridPlane = new Plane(plane.transform.up, plane.transform.position);
        initialObjectScale = objectPrefab.transform.localScale;

        _wallBuilder = new WallBuilder(this);
        // _floorBuilder = new FloorBuilder(this);

        _objectType = objectPrefab.GetComponent<BuildableObject>().objectType;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            if (_objectType == ObjectTypes.Wall)
            {
                _wallBuilder.BuildObject();
            }
            else if (_objectType == ObjectTypes.Floor)
            {
                // _floorBuilder.BuildObject();
            }
        }
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
    Room
}
