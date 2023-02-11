using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A simple intelligence that picks a random interaction and performs it. Best used for Non-Player Characters.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SimpleIntelligence : BaseCharacterIntelligence
{
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>(); // Get the navmesh agent component
    }

    // Update is called once per frame
    public override void Update()
    {
        // If the agent is not performing an interaction and is not moving, pick a random interaction
        if (currentInteraction != null && !_isPerformingInteraction)
        {
            _isPerformingInteraction = true; // Set to true to prevent multiple interactions from being performed
            currentInteraction.PerformInteraction(this, OnInteractionComplete); // Perform the interaction
        }
        else
        {
            if (interactionCooldown <= 0f)
            {
                interactionCooldown = interactionInterval;
                PickRandomInteraction();
            }
            else
            {
                interactionCooldown -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Picks a random interaction from a random smart object
    /// </summary>
    private void PickRandomInteraction()
    {
        // Select random object
        int randomObjectIndex = Random.Range(0, SmartObjectManager.instance.registeredObjects.Count); // Select a random smart object
        SmartObject selectedObject = SmartObjectManager.instance.registeredObjects[randomObjectIndex]; // Get the selected smart object

        // Select random interaction
        int randomInteractionIndex = Random.Range(0, selectedObject.interactions.Count - 1); // Select a random interaction
        BaseInteraction selectedInteraction = selectedObject.interactions[randomInteractionIndex]; // Get the selected interaction

        CheckPerformInteraction(selectedInteraction); // Check if the interaction can be performed
    }
}
