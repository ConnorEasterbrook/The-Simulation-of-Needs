using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacterIntelligence : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int _characterID = 0;
    [HideInInspector] public NavMeshAgent _navMeshAgent = null;
    [HideInInspector] public Communication _individualCommunication = null;

    [Header("Settings")]
    public float interactionInterval = 1f;
    public float interactionCooldown = 0f;
    public BaseInteraction currentInteraction = null;
    [HideInInspector] public bool isPerformingInteraction = false;
    public bool debug = false;

    [Header("Needs")]
    public CharacterNeeds _characterNeedsScript = new CharacterNeeds();
    public CharacterNeedsUI _characterNeedsUIScript = new CharacterNeedsUI();

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>(); // Get the navmesh agent component
    }

    private void Start()
    {
        EstablishCommunication(); // Establish communication
        _characterNeedsScript.Initialize(_individualCommunication); // Initialize the character needs
        _characterNeedsUIScript.Initialize(_individualCommunication, _characterNeedsScript); // Initialize the character needs UI;
    }

    public virtual void EstablishCommunication()
    {
        _individualCommunication = CommunicationManager.instance.GetIndividualCommunication(gameObject); // Get the individual communication
    }

    public Communication GetIndividualCommunication()
    {
        return _individualCommunication;
    }

    /// <summary>
    /// Checks if the interaction can be performed and sets the destination of the navmesh agent to the interaction's position
    /// </summary>
    public virtual void CheckPerformInteraction(BaseInteraction interaction)
    {
        if (interaction.CanPerformInteraction())
        {
            interaction.HeadToInteraction(); // Head to the interaction
            currentInteraction = interaction; // Set the current interaction
            _navMeshAgent.SetDestination(interaction.GetComponent<SmartObject>().interactionPoint); // Set the destination of the navmesh agent to the interaction's position
        }
    }

    /// <summary>
    /// Called when the interaction is complete
    /// </summary>
    public virtual void OnInteractionComplete(BaseInteraction interaction)
    {
        interaction.CompleteInteraction(); // Complete the interaction
        isPerformingInteraction = false; // Set to false to allow the interaction to be performed
        currentInteraction = null;
    }

    public virtual void Update()
    {
        // If the agent is not performing an interaction and is not moving, pick a random interaction
        if (currentInteraction != null && !isPerformingInteraction)
        {
            isPerformingInteraction = true; // Set to true to prevent multiple interactions from being performed
            currentInteraction.PerformInteraction(this, OnInteractionComplete); // Perform the interaction
        }

        // Update the character needs
        _characterNeedsScript.UpdateNeeds();

        // Update the character needs UI
        _characterNeedsUIScript.UpdateSliders();
    }

    public void UpdateIndividualNeed(NeedType needType, float value)
    {
        _characterNeedsScript.UpdateIndividualNeed(needType, value);
    }
}
