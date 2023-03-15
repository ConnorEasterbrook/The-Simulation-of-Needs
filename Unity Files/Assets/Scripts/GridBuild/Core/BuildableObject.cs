using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public float cost = 50.5f;
    public ObjectTypes objectType;

    public float GetCost()
    {
        return cost;
    }
}
