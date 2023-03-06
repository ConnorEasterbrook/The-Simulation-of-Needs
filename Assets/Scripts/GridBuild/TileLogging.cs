using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TileLogging : MonoBehaviour
{
    //? Look into a fancy hashing system to dissect the grid into rooms
    //? This will require the world to be split up into a set amount of chunks instead of a single grid
    //? Spatial hashing is a good way to do this

    [SerializeField] private bool _debug = false;
    [SerializeField] private bool _hasWall = false;
    private GridBuildManager _gridBuildManager;
    private float _tileSize = 0;

    public bool wallOnLeft = false;
    public bool wallOnDown = false;
    public bool wallOnRight = false;
    public bool wallOnUp = false;

    private async void Start()
    {
        _gridBuildManager = GameVariableConnector.instance.transform.GetComponent<GridBuildManager>();
        _tileSize = _gridBuildManager.tileSize;

        await Task.Delay(1000);
        if (Physics.CheckBox(transform.position + new Vector3(0, 1, 0), new Vector3(_tileSize / 2, 1, _tileSize / 2), Quaternion.identity, 1 << 7))
        {
            if (_debug)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
            }

            _hasWall = true;
        }
    }

    public bool HasWall()
    {
        return _hasWall;
    }

    public void AddWall()
    {
        if (!_hasWall)
        {
            if (Physics.CheckBox(transform.position, new Vector3(_tileSize / 2, 1, _tileSize / 2), Quaternion.identity, 1 << 7))
            {
                if (_debug)
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                }

                _hasWall = true;
            }
        }
        else
        {
            return;
        }
    }

    public async void IsCorner()
    {
        await Task.Delay(500);

        // Check in all directions for a wall
        if (Physics.CheckBox(transform.position + new Vector3(-_tileSize / 4, 1, 0), new Vector3(_tileSize / 8, 1, _tileSize / 8), Quaternion.identity, 1 << 7))
        {
            wallOnLeft = true;
        }
        if (Physics.CheckBox(transform.position + new Vector3(0, 1, -_tileSize / 4), new Vector3(_tileSize / 8, 1, _tileSize / 8), Quaternion.identity, 1 << 7))
        {
            wallOnDown = true;
        }
        if (Physics.CheckBox(transform.position + new Vector3(_tileSize / 4, 1, 0), new Vector3(_tileSize / 8, 1, _tileSize / 8), Quaternion.identity, 1 << 7))
        {
            wallOnRight = true;
        }
        if (Physics.CheckBox(transform.position + new Vector3(0, 1, _tileSize / 4), new Vector3(_tileSize / 8, 1, _tileSize / 8), Quaternion.identity, 1 << 7))
        {
            wallOnUp = true;
        }

        int x = Mathf.FloorToInt(transform.position.x + GridBuildManager.gridArray.GetLength(0) / 2);
        int y = Mathf.FloorToInt(transform.position.z);
        Vector2Int startPos = new Vector2Int(x, y);

        // Check if there is a perpendicular corner
        if (wallOnLeft && wallOnDown)
        {
            Vector2Int direction = new Vector2Int(-1, -1);
            ScanRoom(direction, startPos);
        }
        if (wallOnDown && wallOnRight)
        {
            Vector2Int direction = new Vector2Int(1, -1);
            ScanRoom(direction, startPos);
        }
        if (wallOnRight && wallOnUp)
        {
            Vector2Int direction = new Vector2Int(1, 1);
            ScanRoom(direction, startPos);
        }
        if (wallOnUp && wallOnLeft)
        {
            Vector2Int direction = new Vector2Int(-1, 1);
            ScanRoom(direction, startPos);
        }
    }

    private void ScanRoom(Vector2Int direction, Vector2Int startPos)
    {
        RoomScanner _roomScanner = new RoomScanner();
        _roomScanner.SetStartPoint(startPos, direction);
        startPos += direction;
        StartCoroutine(_roomScanner.Test(startPos, direction, this));
    }

    public void CallCoroutine(Vector2Int position, Vector2Int direction, RoomScanner roomScanner, TileLogging tileLogging)
    {
        Vector2Int pos = position + direction;

        StartCoroutine(roomScanner.Test(pos, direction, tileLogging));
    }
}
