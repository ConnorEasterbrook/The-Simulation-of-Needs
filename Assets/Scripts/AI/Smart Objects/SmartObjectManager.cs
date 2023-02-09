using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    [SerializeField] private bool _debug = false;
    public static SmartObjectManager instance = null;
    private List<SmartObject> _registeredObjects = new List<SmartObject>();

    private void Awake()
    {
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

    public void RegisterSmartObject(SmartObject smartObject)
    {
        _registeredObjects.Add(smartObject);

        if (_debug)
        {
            Debug.Log("Registered Smart Object: " + smartObject.displayName);
        }
    }

    public void DeregisterSmartObject(SmartObject smartObject)
    {
        _registeredObjects.Remove(smartObject);

        if (_debug)
        {
            Debug.Log("Deregistered Smart Object: " + smartObject.displayName);
        }
    }
}
