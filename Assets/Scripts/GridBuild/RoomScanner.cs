using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoomScanner
{
    public static GameObject[,] gridArray => GridBuildManager.gridArray;
    public bool[,] gridCheckArray;

    public int scanRange = 14;

    private Vector2Int _startPoint;
    private Vector2Int _direction;

    public void SetStartPoint(Vector2Int _point, Vector2Int _dir)
    {
        _startPoint = _point;
        _direction = _dir;
        gridCheckArray = new bool[gridArray.GetLength(0), gridArray.GetLength(1)];
    }

    private int _invalidCount = 0;
    private int _runningCount = 0;
    private List<GameObject> _roomTiles = new List<GameObject>();
    private List<Vector3> _floorTiles = new List<Vector3>();
    private bool _isRoom = false;

    public IEnumerator Test(Vector2Int startPosition, Vector2Int direction, TileLogging tileLoggingScript)
    {
        if (_startPoint.x > startPosition.x + scanRange || _startPoint.y > startPosition.y + scanRange || _startPoint.x < startPosition.x - scanRange || _startPoint.y < startPosition.y - scanRange)
        {
            // gridArray[startPosition.x, startPosition.y].GetComponent<MeshRenderer>().material.color = Color.red;

            _invalidCount++;
            _roomTiles.Clear();
            yield break;
        }

        _runningCount++;

        if (CheckIfValid(startPosition))
        {
            if (Physics.CheckBox(gridArray[startPosition.x, startPosition.y].transform.position + new Vector3(0, 1, 0), new Vector3(GridBuildManager.getTileSize / 2, 1, GridBuildManager.getTileSize / 2), Quaternion.identity, 1 << 7))
            {
                _runningCount--;
                gridCheckArray[startPosition.x, startPosition.y] = true;
                _roomTiles.Add(gridArray[startPosition.x, startPosition.y]);
                yield break;
            }

            if (!gridCheckArray[startPosition.x, startPosition.y])
            {
                gridCheckArray[startPosition.x, startPosition.y] = true;
                _roomTiles.Add(gridArray[startPosition.x, startPosition.y]);
                // gridArray[startPosition.x, startPosition.y].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

                yield return new WaitForSeconds(0.025f);

                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(1, 0), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(-1, 0), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(0, 1), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(0, -1), this, tileLoggingScript);

                // Diagonals
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(1, 1), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(-1, 1), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(1, -1), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(-1, -1), this, tileLoggingScript);
            }
        }
        else
        {
            _invalidCount++;
            _runningCount--;
            _roomTiles.Clear();
            yield break;
        }

        _runningCount--;
        Debug.Log("Running Count: " + _runningCount + ".");

        if (_invalidCount == 0 && _runningCount == 0 && _roomTiles.Count > 0)
        {
            Debug.Log ("Room Found!");
            FinishScan();
        }
    }

    private void FinishScan()
    {
        // GameObject newRoom = new GameObject();
        // newRoom.name = "Room";
        // newRoom.transform.parent = GameVariableConnector.instance.floorParent.transform;
        // newRoom.transform.position = Vector3.zero;

        foreach (GameObject tile in _roomTiles)
        {
            tile.tag = "Room";
            // CreateFloor(tile, newRoom);
        }
    }

    private void CreateFloor(GameObject tile, GameObject newRoom)
    {
        Vector3 tilePos = new Vector3(tile.transform.position.x + (GridBuildManager.getTileSize / 2), 0.01f, tile.transform.position.z + (GridBuildManager.getTileSize / 2));

        if (!_floorTiles.Contains(tilePos))
        {
            _floorTiles.Add(tilePos);

            GameObject newFloor = new GameObject();
            newFloor.transform.position = tilePos;
            newFloor.transform.localScale = new Vector3(tile.transform.localScale.x, 0.125f, tile.transform.localScale.z);
            newFloor.transform.parent = newRoom.transform;

            // Make sure the newFloor isn't touching a non-room tile && a duplicate
            Collider[] colliders = Physics.OverlapBox(newFloor.transform.position, newFloor.transform.localScale / 4, Quaternion.identity, 1 << 6);
            if (colliders.Length > 0)
            {
                foreach (Collider col in colliders)
                {
                    if (col.gameObject.tag != "Room" && col.gameObject.layer != 8)
                    {
                        GameObject.Destroy(newFloor);
                        break;
                    }
                }
            }

            newFloor.AddComponent<MeshFilter>().mesh = tile.GetComponent<MeshFilter>().mesh;

            MeshRenderer floorRenderer = newFloor.AddComponent<MeshRenderer>();
            floorRenderer.material = tile.GetComponent<MeshRenderer>().material;
            floorRenderer.material.color = Color.blue;

            newFloor.tag = "Floor";
            newFloor.layer = 6;
        }
    }

    public bool isRoom()
    {
        return _isRoom;
    }

    public bool CheckIfValid(Vector2Int _position)
    {
        if (_direction.x > 0)
        {
            if (_position.x + 1 >= gridArray.GetLength(0))
            {
                return false;
            }

            if (_position.x + 1 < _startPoint.x)
            {
                return false;
            }
        }
        else if (_direction.x < 0)
        {
            if (_position.x - 1 < 0)
            {
                return false;
            }

            if (_position.x - 1 > _startPoint.x)
            {
                return false;
            }
        }

        if (_direction.y > 0)
        {
            if (_position.y + 1 >= gridArray.GetLength(1))
            {
                return false;
            }

            if (_position.y + 1 < _startPoint.y)
            {
                return false;
            }
        }
        else if (_direction.y < 0)
        {
            if (_position.y - 1 < 0)
            {
                return false;
            }

            if (_position.y - 1 > _startPoint.y)
            {
                return false;
            }
        }

        return true;
    }

    public void ScanRoom(Vector2 _startPoint, Vector3 _direction)
    {
        // Adjust startpoint for grid offset
        _startPoint.x += gridArray.GetLength(0) / 2;

        gridCheckArray = new bool[scanRange, scanRange];

        int initialX = Mathf.FloorToInt(_startPoint.x);
        int initialY = Mathf.FloorToInt(_startPoint.y);

        int _x = Mathf.FloorToInt(_startPoint.x);
        int _y = Mathf.FloorToInt(_startPoint.y);

        int xTracker = 0;
        int yTracker = 0;

        for (int x = 0; x < scanRange; x++)
        {
            // Go in the given direction
            if (_direction.x > 0)
            {
                _x--;
            }
            if (_direction.x < 0)
            {
                _x++;
            }

            for (int y = 0; y < scanRange; y++)
            {
                // Go in the given direction
                if (_direction.z > 0)
                {
                    _y--;
                }
                if (_direction.z < 0)
                {
                    _y++;
                }

                if (CheckForWalls(_x, _y))
                {
                    break;
                }

                if (gridArray[_x, _y] != null)
                {
                    gridArray[_x, _y].GetComponent<MeshRenderer>().material.color = Color.yellow;
                }

                gridCheckArray[x, y] = true;

                yTracker++;
            }

            xTracker++;
            _y = initialY;
        }

        _x = initialX;

        if (xTracker >= scanRange - 1 || yTracker >= scanRange - 1)
        {
            Debug.Log("No Room Found");
        }
        else
        {
            Debug.Log("Room Found");
        }
    }

    private bool CheckForWalls(int x, int y)
    {
        // Check if there's a wall gameobject in the way by checking collisions at the top of the tile
        if (Physics.CheckBox(gridArray[x, y].transform.position + new Vector3(0, 1, 0), new Vector3(GridBuildManager.getTileSize / 2, 1, GridBuildManager.getTileSize / 2), Quaternion.identity, 1 << 7))
        {
            return true;
        }

        return false;
    }
}
