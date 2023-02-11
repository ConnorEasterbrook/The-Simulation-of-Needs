using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all smart objects in the scene.
/// </summary>
public class SmartObjectManager : MonoBehaviour
{
    [SerializeField] private bool _debug = false; // Whether or not to display debug messages
    public static SmartObjectManager instance = null; // The singleton instance of the SmartObjectManager
    public List<SmartObject> registeredObjects = new List<SmartObject>(); // The list of registered smart objects

    private void Awake()
    {
        // If an instance of the SmartObjectManager already exists, destroy this instance
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// Registers a smart object
    /// </summary>
    public void RegisterSmartObject(SmartObject smartObject)
    {
        registeredObjects.Add(smartObject);

        if (_debug)
        {
            Debug.Log("Registered Smart Object: " + smartObject.displayName);
        }
    }

    /// <summary>
    /// Deregisters a smart object
    /// </summary>
    public void DeregisterSmartObject(SmartObject smartObject)
    {
        registeredObjects.Remove(smartObject);

        if (_debug)
        {
            Debug.Log("Deregistered Smart Object: " + smartObject.displayName);
        }
    }
}