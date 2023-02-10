using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleIntelligence : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public float interactionInterval = 5f;
    public float interactionCooldown = 0f;
    [HideInInspector] public BaseInteraction currentInteraction;
    private bool _isPerformingInteraction = false;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>(); // Get the navmesh agent component
    }

    // Update is called once per frame
    void Update()
    {
        // If the agent is not performing an interaction and is not moving, pick a random interaction
        if (currentInteraction != null && !_isPerformingInteraction)
        {
            _isPerformingInteraction = true; // Set to true to prevent multiple interactions from being performed
            currentInteraction.PerformInteraction(gameObject, OnInteractionComplete); // Perform the interaction
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

            // // If the agent is not performing an interaction and is moving, check if the agent has reached the interaction's position
            // if (currentInteraction != null && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            // {
            //     // If the agent has reached the interaction's position, check if the interaction can be performed
            //     CheckPerformInteraction(currentInteraction);
            // }
            // else
            // {
            //     // If the agent is not performing an interaction and is not moving, pick a random interaction
            //     if (currentInteraction == null && !_navMeshAgent.hasPath)
            //     {
            //         PickRandomInteraction();
            //     }
            // }
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

    /// <summary>
    /// Checks if the interaction can be performed and sets the destination of the navmesh agent to the interaction's position
    /// </summary>
    private void CheckPerformInteraction(BaseInteraction interaction)
    {
        if (interaction.CanPerformInteraction())
        {
            _isPerformingInteraction = false; // Set to false to allow the interaction to be performed
            currentInteraction = interaction; // Set the current interaction
            _navMeshAgent.SetDestination(interaction.GetComponent<SmartObject>().interactionPoint); // Set the destination of the navmesh agent to the interaction's position
        }
    }

    /// <summary>
    /// Called when the interaction is complete
    /// </summary>
    private void OnInteractionComplete(BaseInteraction interaction)
    {
        currentInteraction = null;
    }
}
