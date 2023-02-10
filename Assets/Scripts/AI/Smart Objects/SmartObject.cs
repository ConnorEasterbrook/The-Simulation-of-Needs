using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleInteraction))]
public class SmartObject : MonoBehaviour
{
    public string displayName; // The name of the smart object

    // The list of interactions that can be performed on the smart object. Separated from the property for prefab assignment
    private List<BaseInteraction> _interactions = null; // The list of interactions that can be performed on the smart object
    public List<BaseInteraction> interactions => _interactions != null ? _interactions : _interactions = new List<BaseInteraction>(GetComponents<BaseInteraction>()); // The list of interactions that can be performed on the smart object

    // The point where the interaction will take place. Serialized for prefab assignment
    [SerializeField] private Transform _interactionPoint; // The point where the interaction will take place
    public Vector3 interactionPoint => _interactionPoint.position != null ? _interactionPoint.position : transform.position; // The position of the interaction point

    // Start is called before the first frame update
    void Start()
    {
        SmartObjectManager.instance.RegisterSmartObject(this);
    }

    private void OnDestroy()
    {
        SmartObjectManager.instance.DeregisterSmartObject(this);
    }
}
