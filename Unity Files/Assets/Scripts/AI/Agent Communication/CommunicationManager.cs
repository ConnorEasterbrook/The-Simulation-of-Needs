using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    public static CommunicationManager instance = null; // The instance of the SmartObjectManager
    private Dictionary<GameObject, Communication> _individualCommunication = new Dictionary<GameObject, Communication>(); // The list of all individual communication
    private Dictionary<int, Communication> _sharedCommunication = new Dictionary<int, Communication>(); // The list of all shared communication

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

    public Communication GetIndividualCommunication(GameObject requester)
    {
        if (!_individualCommunication.ContainsKey(requester))
        {
            _individualCommunication[requester] = new Communication();
        }

        return _individualCommunication[requester];
    }

    public Communication GetSharedCommunication(int requesterID)
    {
        if (!_sharedCommunication.ContainsKey(requesterID))
        {
            _sharedCommunication[requesterID] = new Communication();
        }

        return _sharedCommunication[requesterID];
    }
}
