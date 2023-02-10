using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
