using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class GridBuildManager : MonoBehaviour
{
    public static bool isBuilding = false;
    public static GridBuildCore _gridBuildCore = new GridBuildCore();

    public GameObject objectPrefab;
    public GameObject previewObject;
    public float tileSize = 1f;

    private ObjectTypes _objectType;
    private WallBuilder _wallBuilder = new WallBuilder();
    private FloorBuilder _floorBuilder = new FloorBuilder();
    private RoomObject _roomBuilder;
    private GroundFurnitureBuild _groundFurnitureBuilder = new GroundFurnitureBuild();
    private DeskFurnitureBuild _deskFurnitureBuilder = new DeskFurnitureBuild();

    private GameVariableConnector _gameVariableConnector;

    [SerializeField] private GameObject tilePrefab;
    public GameObject[,] gridArray;
    public int gridX;
    public int gridY;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid(gridX, gridY);

        _gameVariableConnector = GameVariableConnector.instance;

        Plane _gridPlane = new Plane(Vector3.up, Vector3.zero);

        // gridPlane = new Plane(plane.transform.up, plane.transform.position);
        _gridBuildCore.PrepVariables(_gameVariableConnector, objectPrefab, previewObject, _gridPlane, tileSize);
        _objectType = objectPrefab.GetComponent<BuildableObject>().objectType;

        // Bake navmesh
        NavMeshSurface surface = tilePrefab.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();


        int x = Random.Range(0, gridX);
        int y = Random.Range(0, gridY);
        StartCoroutine(FloodFill(x, y, Color.white, Color.red));
    }

    public void CreateGrid(int _x, int _y, GameObject tile = null)
    {
        if (tile == null)
        {
            tile = tilePrefab;
        }

        tile.isStatic = true;

        gridArray = new GameObject[_x, _y];
        Vector2 startPos = this.transform.position;

        for (int x = 0; x < _x; x++)
        {
            for (int y = 0; y < _y; y++)
            {
                GameObject tileGO = new GameObject();
                tileGO.layer = 6;
                tileGO.AddComponent<MeshFilter>().mesh = tile.GetComponent<MeshFilter>().sharedMesh;

                MeshRenderer meshRenderer = tile.GetComponent<MeshRenderer>();
                meshRenderer.material.color = Color.white;
                tileGO.AddComponent<MeshRenderer>().material = meshRenderer.sharedMaterial;

                tileGO.transform.localScale = new Vector3(tileSize, .25f, tileSize);
                tileGO.transform.position = new Vector3(startPos.x + (x * tileSize), -(tilePrefab.transform.localScale.y / 2f), startPos.y + (y * tileSize));
                tileGO.transform.parent = this.transform;
                tileGO.name = "Tile_" + x + "_" + y;
                gridArray[x, y] = tileGO;
            }
        }

        transform.position = new Vector3(transform.position.x - (gridX * tileSize) / 2, transform.position.y, transform.position.z);
        // tilePrefab.SetActive(false);
    }

    public IEnumerator FloodFill(int x, int y, Color oldColour, Color newColour)
    {
        if (x < 0 || x >= gridX || y < 0 || y >= gridY)
        {
            yield break;
        }

        if (gridArray[x, y].GetComponent<MeshRenderer>().material.color == oldColour)
        {
            gridArray[x, y].GetComponent<MeshRenderer>().material.color = newColour;
            yield return new WaitForSeconds(.1f);
            StartCoroutine(FloodFill(x + 1, y, oldColour, newColour));
            StartCoroutine(FloodFill(x - 1, y, oldColour, newColour));
            StartCoroutine(FloodFill(x, y + 1, oldColour, newColour));
            StartCoroutine(FloodFill(x, y - 1, oldColour, newColour));

            // Get diagonal neighbours
            StartCoroutine(FloodFill(x + 1, y + 1, oldColour, newColour));
            StartCoroutine(FloodFill(x - 1, y + 1, oldColour, newColour));
            StartCoroutine(FloodFill(x + 1, y - 1, oldColour, newColour));
            StartCoroutine(FloodFill(x - 1, y - 1, oldColour, newColour));
        }
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
