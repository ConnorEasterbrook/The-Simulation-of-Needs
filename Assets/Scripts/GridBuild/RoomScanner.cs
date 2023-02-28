using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoomScanner : MonoBehaviour
{
    //TODO Room Detection
    //? Only call this script on a single tile that did not already detect a wall object (new wall).
    //? If the tile detects a wall object, scan along the wall in the direction of the wall object and check if there is a wall an acute angle away from the wall object.
    //? The maximum range should be 20 tiles. If there are no corners within that range then cancel the script call.
    //? Go along the walls and corners, temporarily checking a boolean to make sure that the script does not go back to the same tile. If the tiles lead to a tile that has already been checked when going along the wall, then a room has been found.
    //? If a room has been found then place all the walls and floor tiles into its own game object and set the boolean to false for all the tiles in the room.

    [SerializeField] private bool _debug = false;
    [SerializeField] private bool _hasWall = false;
    private GridBuildManager _gridBuildManager;
    private float _tileSize = 0;

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
            if (Physics.CheckBox(transform.position + new Vector3(0, 1, 0), new Vector3(_tileSize / 2, 1, _tileSize / 2), Quaternion.identity, 1 << 7))
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

    public void CheckRoom()
    {

    }
}
