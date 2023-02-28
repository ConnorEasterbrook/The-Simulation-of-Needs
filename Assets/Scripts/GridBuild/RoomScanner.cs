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

    public IEnumerator CheckFloodFill(int x, int y, TileLogging tileLoggingScript)
    {
        // Make sure the tile is within the grid
        if (x < 0 || x >= gridArray.GetLength(0) || y < 0 || y >= gridArray.GetLength(1))
        {
            yield break;
        }

        // Check if there's a wall gameobject in the way by checking collisions at the top of the tile
        if (Physics.CheckBox(gridArray[x, y].transform.position + new Vector3(0, 1, 0), new Vector3(GridBuildManager.getTileSize / 2, 1, GridBuildManager.getTileSize / 2), Quaternion.identity, 1 << 7))
        {

            gridCheckArray[x, y] = true;
            gridArray[x, y].GetComponent<MeshRenderer>().material.color = Color.yellow;

            yield break;
        }

        if (!gridCheckArray[x, y])
        {
            gridCheckArray[x, y] = true;
            gridArray[x, y].GetComponent<MeshRenderer>().material.color = Color.green;

            // if (x == scanRange - 1 && y == scanRange - 1)
            // {
            //     // instance.GetWorldInfo(gridArray, gridCheckArray, wallTiles);
            //     bool foundUncheckedTile = false;

            //     for (int i = 0; i < scanRange; i++)
            //     {
            //         for (int j = 0; j < scanRange; j++)
            //         {
            //             if (gridCheckArray[i, j] == false)
            //             {
            //                 foundUncheckedTile = true;
            //                 break;
            //             }
            //         }

            //         if (foundUncheckedTile)
            //         {
            //             break;
            //         }
            //     }
            // }

            yield return new WaitForSeconds(0.1f);

            // tileLoggingScript.CallCoroutine(x + 1, y, tileLoggingScript);
            // tileLoggingScript.CallCoroutine(x - 1, y, tileLoggingScript);
            // tileLoggingScript.CallCoroutine(x, y + 1, tileLoggingScript);
            // tileLoggingScript.CallCoroutine(x, y - 1, tileLoggingScript);
        }
    }

    private int _invalidCount = 0;
    private int _runningCount = 0;
    private bool _done = false;

    public IEnumerator Test(Vector2Int startPosition, Vector2Int direction, TileLogging tileLoggingScript)
    {
        if (_startPoint.x > startPosition.x + scanRange || _startPoint.y > startPosition.y + scanRange || _startPoint.x < startPosition.x - scanRange || _startPoint.y < startPosition.y - scanRange)
        {
            gridArray[startPosition.x, startPosition.y].GetComponent<MeshRenderer>().material.color = Color.red;

            _invalidCount++;
            yield break;
        }
        
        _runningCount++;

        if (CheckIfValid(startPosition))
        {
            if (Physics.CheckBox(gridArray[startPosition.x, startPosition.y].transform.position + new Vector3(0, 1, 0), new Vector3(GridBuildManager.getTileSize / 2, 1, GridBuildManager.getTileSize / 2), Quaternion.identity, 1 << 7))
            {
                _runningCount--;
                yield break;
            }

            if (!gridCheckArray[startPosition.x, startPosition.y])
            {
                gridCheckArray[startPosition.x, startPosition.y] = true;
                // gridArray[startPosition.x, startPosition.y].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

                yield return new WaitForSeconds(0.1f);

                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(1, 0), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(-1, 0), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(0, 1), this, tileLoggingScript);
                tileLoggingScript.CallCoroutine(startPosition, new Vector2Int(0, -1), this, tileLoggingScript);
            }
        }
        else
        {
            _invalidCount++;
            _runningCount--;
            yield break;
        }


        _runningCount--;

        if (_invalidCount == 0 && _runningCount <= 1 && !_done)
        {
            _done = true;
            Debug.Log("We have a room!");
        }
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
