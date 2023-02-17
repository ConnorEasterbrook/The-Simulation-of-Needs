using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildableObject))]
public class WallObject : MonoBehaviour
{
    public static List<WallObject> wallObjects = new List<WallObject>();
    public List<WallObject> neighbors = new List<WallObject>();
    public RoomObject[] rooms = new RoomObject[2];
    public int wallID;

    // Start is called before the first frame update
    void Start()
    {
        wallObjects.Add(this);
        wallID = wallObjects.Count;
    }
}
