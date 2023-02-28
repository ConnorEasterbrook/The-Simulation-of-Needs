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
    public static float getTileSize;

    private ObjectTypes _objectType;
    private WallBuilder _wallBuilder = new WallBuilder();
    private FloorBuilder _floorBuilder = new FloorBuilder();
    private RoomObject _roomBuilder;
    private GroundFurnitureBuild _groundFurnitureBuilder = new GroundFurnitureBuild();
    private DeskFurnitureBuild _deskFurnitureBuilder = new DeskFurnitureBuild();

    private GameVariableConnector _gameVariableConnector;

    [SerializeField] private GameObject tilePrefab;
    public static GameObject[,] gridArray;
    public bool[,] gridCheckArray;
    public int gridX;
    public int gridY;
    public List<Vector2> wallTiles = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        WorldGenerator worldGenerator = gameObject.GetComponent<WorldGenerator>();
        worldGenerator.Initialize(this, (int)tileSize, tilePrefab, gridX, gridY);

        getTileSize = tileSize;
        gridArray = new GameObject[gridX, gridY];
        gridCheckArray = new bool[gridX, gridY];

        _gameVariableConnector = GameVariableConnector.instance;

        _objectType = objectPrefab.GetComponent<BuildableObject>().objectType;
    }

    public void GetWorldInfo(GameObject[,] _gridArray, bool[,] _gridCheckArray, List<Vector2> _wallTiles)
    {
        gridArray = _gridArray;
        gridCheckArray = _gridCheckArray;
        wallTiles = _wallTiles;

        // Bake navmesh
        NavMeshSurface surface = tilePrefab.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        Plane _gridPlane = new Plane(Vector3.up, Vector3.zero);
        _gridBuildCore.PrepVariables(_gameVariableConnector, objectPrefab, previewObject, _gridPlane, tileSize, gridArray, gridCheckArray);

        GameVariableConnector.instance.UnpauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if raycast is hitting a room tag object
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (hit.collider.gameObject.tag == "Room")
            {
                _gridBuildCore.UpdateInRoom(true);
            }
            else
            {
                _gridBuildCore.UpdateInRoom(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Floodfilling");
            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    if (gridCheckArray[x, y])
                    {
                        continue;
                    }

                    // Check if there's a wall gameobject in the way by checking collisions at the top of the tile
                    if (Physics.CheckBox(gridArray[x, y].transform.position + new Vector3(0, 1, 0), new Vector3(tileSize / 2, 1, tileSize / 2), Quaternion.identity, 1 << 7))
                    {
                        if (!wallTiles.Contains(new Vector2(x, y)))
                        {
                            wallTiles.Add(new Vector2(x, y));
                        }

                        gridArray[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
                        gridCheckArray[x, y] = true;
                        continue;
                    }

                    gridArray[x, y].GetComponent<MeshRenderer>().material.color = Color.yellow;
                    gridCheckArray[x, y] = true;
                }
            }
        }

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
