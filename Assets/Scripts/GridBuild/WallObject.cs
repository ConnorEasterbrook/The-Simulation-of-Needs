using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildableObject))]
public class WallObject : MonoBehaviour
{
    public static List<WallObject> wallObjects = new List<WallObject>();
    public List<WallObject> neighbourWalls = new List<WallObject>();
    public RoomObject[] rooms = new RoomObject[2];
    public int wallID;
    public bool visited = false;

    // Start is called before the first frame update
    void Start()
    {
        wallObjects.Add(this);
        wallID = wallObjects.Count;

        StartCoroutine(CheckForNeighborOnceAfterDelay(0.5f));
    }
    
    private IEnumerator CheckForNeighborOnceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckForNeighbour();
    }

    private void CheckForNeighbour()
    {
        // Check if neighbour from box collider
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z), transform.localRotation);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<WallObject>() != null)
            {
                // Continue if the collider is this wall
                if (hitCollider.gameObject == gameObject)
                {
                    continue;
                }

                // Check if this wall has the connected wall as a neighbour
                if (!neighbourWalls.Contains(hitCollider.gameObject.GetComponent<WallObject>()))
                {
                    neighbourWalls.Add(hitCollider.gameObject.GetComponent<WallObject>());
                }

                // Check if the connected wall has this wall as a neighbour
                if (!hitCollider.gameObject.GetComponent<WallObject>().neighbourWalls.Contains(this))
                {
                    hitCollider.gameObject.GetComponent<WallObject>().neighbourWalls.Add(this);
                }
            }
        }

        // Modify list from position
        for (int i = 0; i < neighbourWalls.Count; i++)
        {
            if (neighbourWalls[i].transform.localRotation != transform.localRotation)
            {
                // Set to first neighbour and move others up one
                WallObject temp = neighbourWalls[0];
                neighbourWalls[0] = neighbourWalls[i];
                neighbourWalls[i] = temp;
            }
        }

        // TraverseConnectedWalls(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.localRotation, transform.localScale);
    }

    private void Update()
    {
    }

    private List<WallObject> TraverseConnectedWalls(WallObject wall)
    {
        List<WallObject> connectedWalls = new List<WallObject>();
        connectedWalls.Add(wall);

        foreach (WallObject connectedWall in wall.neighbourWalls)
        {
            if (!connectedWalls.Contains(connectedWall))
            {
                connectedWalls.AddRange(TraverseConnectedWalls(connectedWall));
                Debug.Log ("Added " + connectedWall.name + " to connected walls");
            }
        }

        return connectedWalls;
    }

    public class WallObjectData
    {
        public int wallID;
        public List<WallObject> connectedWalls = new List<WallObject>();
        public int[] rooms;
        public bool visited = false;
    }
}
