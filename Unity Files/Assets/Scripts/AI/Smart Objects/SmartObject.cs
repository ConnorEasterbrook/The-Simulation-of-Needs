using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smart objects are objects that can be interacted with. They are registered with the SmartObjectManager.
/// </summary>
[RequireComponent(typeof(SimpleInteraction))]
public class SmartObject : MonoBehaviour
{
    public string displayName; // The name of the smart object

    // The list of interactions that can be performed on the smart object. Separated from the property for prefab assignment
    private List<BaseInteraction> _interactions = null; // The list of interactions that can be performed on the smart object
    public List<BaseInteraction> interactions => _interactions != null ? _interactions : _interactions = new List<BaseInteraction>(GetComponents<BaseInteraction>()); // The list of interactions that can be performed on the smart object

    // The point where the interaction will take place. Serialized for prefab assignment
    public Transform[] interactionPoints; // The point where the interaction will take place
    public bool[] interactionPointsOccupied; // The point where the interaction will take place
    public int[] interactionPointOccupierID; // The point where the interaction will take place

    // Start is called before the first frame update
    void Start()
    {
        if (interactionPoints == null)
        {
            interactionPoints = new Transform[1];
            interactionPoints[0] = transform;
        }

        interactionPointsOccupied = new bool[interactionPoints.Length];
        interactionPointOccupierID = new int[interactionPoints.Length];
        
        SmartObjectManager.instance.RegisterSmartObject(this);

        if (GetComponent<BaseInteraction>().interactionType == InteractionType.Work)
        {
            SmartObjectManager.instance.RegisterWorkObject(this, true);
        }
    }

    private void OnDestroy()
    {
        SmartObjectManager.instance.DeregisterSmartObject(this);

        if (GetComponent<BaseInteraction>().interactionType == InteractionType.Work)
        {
            SmartObjectManager.instance.RegisterWorkObject(this, false);
        }
    }

    public Transform GetInteractionPoint(int performerID)
    {
        for (int i = 0; i < interactionPoints.Length; i++)
        {
            if (!interactionPointsOccupied[i])
            {
                interactionPointsOccupied[i] = true;
                interactionPointOccupierID[i] = performerID;

                // Debug.Log("Interaction point " + i + " is now occupied." + " Occupier ID: " + performerID + " | " + interactionPointOccupierID[i]);
                return interactionPoints[i];
            }
        }
        
        if (interactionPointsOccupied[0] && interactionPoints.Length > 1)
        {
            return interactionPoints[1];
        }
        else
        {
            return interactionPoints[0];
        }
    }
}
