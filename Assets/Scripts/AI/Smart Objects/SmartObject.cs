using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    public string displayName;
    public List<BaseInteraction> _interactions = new List<BaseInteraction>();

    // Start is called before the first frame update
    void Start()
    {
        SmartObjectManager.instance.RegisterSmartObject(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        SmartObjectManager.instance.DeregisterSmartObject(this);
    }
}
