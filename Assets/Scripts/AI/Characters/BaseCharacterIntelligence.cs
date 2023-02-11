using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacterIntelligence : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent _navMeshAgent = null;
    public float interactionInterval = 5f;
    public float interactionCooldown = 0f;
    [HideInInspector] public BaseInteraction currentInteraction = null;
    [HideInInspector] public bool _isPerformingInteraction = false;
    public CharacterNeeds _characterNeedsScript = new CharacterNeeds();
    public CharacterNeedsUI _characterNeedsUIScript = new CharacterNeedsUI();

    /// <summary>
    /// Checks if the interaction can be performed and sets the destination of the navmesh agent to the interaction's position
    /// </summary>
    public virtual void CheckPerformInteraction(BaseInteraction interaction)
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
    public virtual void OnInteractionComplete(BaseInteraction interaction)
    {
        currentInteraction = null;
    }

    public virtual void Update()
    {
        // If the agent is not performing an interaction and is not moving, pick a random interaction
        if (currentInteraction != null && !_isPerformingInteraction)
        {
            _isPerformingInteraction = true; // Set to true to prevent multiple interactions from being performed
            currentInteraction.PerformInteraction(this, OnInteractionComplete); // Perform the interaction
        }

        // Update the character needs
        _characterNeedsScript.UpdateNeeds();

        // Update the character needs UI
        _characterNeedsUIScript.UpdateSliders();
    }

    public void UpdateIndividualNeed(NeedType needType, float value)
    {
        Debug.Log("Updating " + needType + " to " + value);
        _characterNeedsScript.UpdateIndividualNeed(needType, value);
    }
}
