using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A more complex intelligence that picks interactions based on needs and performs it. Best used for Autonomous Agents like Sims.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AutonomousIntelligence : BaseCharacterIntelligence
{
    // private float _interactionInterval = 5f; // The interval between interactions
    // private float _interactionCooldown = 0f; // The cooldown between interactions
    private float _defaultInteractionScore = 0f; // The default score for an interaction

    public override void Update()
    {
        // base.Update();

        // // If the agent is not performing an interaction and is not moving, pick a random interaction
        // if (currentInteraction == null)
        // {
        //     // _isPerformingInteraction = true; // Set to true to prevent multiple interactions from being performed
        //     // currentInteraction.PerformInteraction(this, OnInteractionComplete); // Perform the interaction

        //     _interactionCooldown -= Time.deltaTime; // Decrement the interaction cooldown

        //     if (_interactionCooldown <= 0f)
        //     {
        //         _interactionCooldown = _interactionInterval; // Reset the interaction cooldown
        //         PickBestInteractiom(); // Pick the best interaction
        //     }
        // }

        // If the agent is not performing an interaction and is not moving, pick a random interaction
        if (currentInteraction != null && !isPerformingInteraction)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                isPerformingInteraction = true; // Set to true to prevent multiple interactions from being performed
                currentInteraction.PerformInteraction(this, OnInteractionComplete); // Perform the interaction
            }
        }
        else
        {
            if (currentInteraction == null)
            {
                interactionCooldown = interactionInterval;
                PickBestInteractiom();
            }
        }

        // Update the character needs
        _characterNeedsScript.UpdateNeeds();

        // Update the character needs UI
        _characterNeedsUIScript.UpdateSliders();
    }

    /// <summary>
    /// Selects the best interaction to perform based on the needs of the character
    /// </summary>
    private void PickBestInteractiom()
    {
        List<ScoredInteraction> scoredInteractionsUnsorted = new List<ScoredInteraction>(); // The list of scored interactions

        if (_characterNeedsScript.AreNeedsFine())
        {
            foreach (var workObject in SmartObjectManager.instance.workObjects)
            {
                foreach (var interaction in workObject.interactions)
                {
                    if (!interaction.CanPerformInteraction())
                    {
                        continue;
                    }

                    scoredInteractionsUnsorted.Add(new ScoredInteraction(workObject, interaction, ScoreInteraction(interaction))); // Add the scored interaction to the list
                }
            }

            if (scoredInteractionsUnsorted.Count == 0)
            {
                GetNeedInteraction(scoredInteractionsUnsorted);
            }
        }
        else
        {
            GetNeedInteraction(scoredInteractionsUnsorted);
        }

        if (scoredInteractionsUnsorted.Count == 0)
        {
            return;
        }

        // Sort the scored interactions by score in descending order. The first interaction will be the best interaction
        List<ScoredInteraction> scoredInteractionsSorted = scoredInteractionsUnsorted.OrderByDescending(scoredInteraction => scoredInteraction.interactionScore).ToList();
        // SmartObject selectedObject = scoredInteractionsSorted[0].interactionObject; // Get the selected smart object

        for (int i = 0; i < scoredInteractionsSorted.Count; i++)
        {
            if (scoredInteractionsSorted[i].interaction.CanPerformInteraction())
            {
                if (debug) Debug.Log("Amount of interactions: " + scoredInteractionsSorted.Count);
                BaseInteraction interaction = scoredInteractionsSorted[i].interaction; // Get the selected interaction
                interaction.HeadToInteraction(); // Head to the interaction
                currentInteraction = interaction; // Set the current interaction
                _navMeshAgent.SetDestination(interaction.GetComponent<SmartObject>().interactionPoint); // Set the destination of the navmesh agent to the interaction's position
                break;
            }
        }

        // BaseInteraction selectedInteraction = scoredInteractionsSorted[0].interaction; // Get the selected interaction

        // CheckPerformInteraction(selectedInteraction); // Check if the interaction can be performed
    }

    private void GetNeedInteraction(List<ScoredInteraction> scoredInteractionsUnsorted)
    {
        foreach (var smartObject in SmartObjectManager.instance.registeredObjects)
        {
            foreach (var interaction in smartObject.interactions)
            {
                if (!interaction.CanPerformInteraction())
                {
                    continue;
                }

                scoredInteractionsUnsorted.Add(new ScoredInteraction(smartObject, interaction, ScoreInteraction(interaction))); // Add the scored interaction to the list
            }
        }
    }

    /// <summary>
    /// Scores an interaction based on the needs of the character
    /// </summary>
    private float ScoreInteraction(BaseInteraction interaction)
    {
        // If the interaction has no need changes, return the default score
        if (interaction.needsChanges.Count == 0)
        {
            return _defaultInteractionScore;
        }

        float interactionScore = 0f;

        // Calculate the score for each need change 
        foreach (var needChange in interaction.needsChanges)
        {
            interactionScore += ScoreChange(needChange.targetNeedType, needChange.changeAmount); // Add the score change to the interaction score
        }

        return interactionScore; // Return the interaction score
    }

    /// <summary>
    /// Calaculates the score change for a need
    /// </summary>
    private float ScoreChange(NeedType needType, float changeAmount)
    {
        float currentNeedValue = 100 - _characterNeedsScript.GetNeedValue(needType); // Get the current need value by subtracting the current need value from 100
        // float newNeedValue = currentNeedValue + changeAmount; // Calculate the new need value
        Mathf.Clamp(currentNeedValue, 0f, 100f);

        return currentNeedValue;
    }

    private class ScoredInteraction
    {
        public SmartObject interactionObject;
        public BaseInteraction interaction;
        public float interactionScore;

        public ScoredInteraction(SmartObject interactionObject, BaseInteraction interaction, float interactionScore)
        {
            this.interactionObject = interactionObject;
            this.interaction = interaction;
            this.interactionScore = interactionScore;
        }
    }
}
