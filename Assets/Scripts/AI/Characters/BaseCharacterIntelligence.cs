using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseCharacterIntelligence : MonoBehaviour
{
    public string characterName;
    private static List<BaseCharacterIntelligence> _performers = new List<BaseCharacterIntelligence>();

    public class NameGen
    {
        public List<string> names = new List<string>();
    }


    [Header("General")]
    public int characterID;
    [HideInInspector] public NavMeshAgent navMeshAgent = null;
    [HideInInspector] public Communication individualCommunication = null;
    [HideInInspector] public bool rotatePerformer = false;

    [Header("Settings")]
    public float interactionInterval = 1f;
    public float interactionCooldown = 0f;
    public BaseInteraction currentInteraction = null;
    [HideInInspector] public bool isPerformingInteraction = false;
    public bool debug = false;

    [Header("Needs")]
    public CharacterNeeds characterNeedsScript = new CharacterNeeds();
    public CharacterSkills characterSkillsScript = new CharacterSkills();

    [Header("Traits")]
    public List<Trait> traits = new List<Trait>();

    private void Awake()
    {
        _performers.Add(this);
        characterID = _performers.Count;

        navMeshAgent = GetComponent<NavMeshAgent>(); // Get the navmesh agent component
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        currentInteraction = null;

        EstablishCommunication(); // Establish communication
        characterNeedsScript.Initialize(individualCommunication); // Initialize the character needs

        GenerateName();
        traits = characterSkillsScript.GenerateTraits(gameObject.GetComponent<AutonomousIntelligence>());
        // traits = null;

        CharacterNeedsUI characterNeedsUI = GameVariableConnector.instance.generalGUIManagerScript._characterNeedsUIScript;
        characterNeedsUI.AddPerformer(gameObject.GetComponent<AutonomousIntelligence>());
    }

    public void GenerateName()
    {
        TextAsset namesJson = new TextAsset("Hello");
        namesJson = GameVariableConnector.instance.GetNamesJson();

        if (namesJson == null)
        {
            Debug.LogError("No names.json file found!");
            gameObject.name = characterName;
            return;
        }

        var classNames = JsonUtility.FromJson<NameGen>(namesJson.text);
        var forename = classNames.names[Random.Range(0, classNames.names.Count)];
        var surname = classNames.names[Random.Range(0, classNames.names.Count)];
        gameObject.name = forename + " " + surname;
        characterName = gameObject.name;

        // CharacterNeedsUI characterNeedsUI = GameVariableConnector.instance.generalGUIManagerScript._characterNeedsUIScript;
        // characterNeedsUI.AddPerformer(gameObject.GetComponent<AutonomousIntelligence>());
    }

    public virtual void EstablishCommunication()
    {
        individualCommunication = CommunicationManager.instance.GetIndividualCommunication(gameObject); // Get the individual communication
    }

    public Communication GetIndividualCommunication()
    {
        return individualCommunication;
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
            navMeshAgent.SetDestination(interaction.GetComponent<SmartObject>().GetInteractionPoint(characterID).position); // Set the destination of the navmesh agent to the interaction's position
        }
    }

    /// <summary>
    /// Called when the interaction is complete
    /// </summary>
    public virtual void OnInteractionComplete(BaseInteraction interaction)
    {
        interaction.CompleteInteraction(characterID); // Complete the interaction
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
        characterNeedsScript.UpdateNeeds();
    }

    public void UpdateIndividualNeed(NeedType needType, float value)
    {
        characterNeedsScript.UpdateIndividualNeed(needType, value);
    }
}
