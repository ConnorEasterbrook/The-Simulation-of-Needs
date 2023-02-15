using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <Summary>
/// Simple interaction that can be performed by multiple people at the same time.
/// </Summary>
public class SimpleInteraction : BaseInteraction
{
    public int maxSimultaneousInteractions = 1; // The maximum amount of people that can perform the interaction at the same time
    public List<PerformerInformation> performers = new List<PerformerInformation>(); // The list of performers
    public int _currentInteractions = 0; // The current amount of people performing the interaction

    /// <summary>
    /// Returns true if the interaction can be performed.
    /// </summary>
    public override bool CanPerformInteraction()
    {
        return _currentInteractions < maxSimultaneousInteractions;
    }

    public override void HeadToInteraction()
    {
        _currentInteractions++; // Increment the current amount of peorple performing the interaction
    }

    /// <summary>
    /// Performs the interaction.
    /// </summary>
    public override void PerformInteraction(BaseCharacterIntelligence performer, UnityAction<BaseInteraction> onInteractionComplete)
    {
        // Check the interaction type and perform the interaction
        switch (interactionType)
        {
            case InteractionType.Instant:
                InstantInteraction(performer, onInteractionComplete);
                break;

            case InteractionType.OverTime:
                OverTimeInteraction(performer, onInteractionComplete);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Cancels the interaction.
    /// </summary>
    public override void CancelInteraction()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Performs the interaction instantly.
    /// </summary>
    private void InstantInteraction(BaseCharacterIntelligence performer, UnityAction<BaseInteraction> onInteractionComplete)
    {
        if (needsChanges.Count > 0)
        {
            ApplyNeedsChanges(performer, 1f);
        }

        onInteractionComplete.Invoke(this);
    }

    /// <summary>
    /// Performs the interaction over time.
    /// </summary>
    private void OverTimeInteraction(BaseCharacterIntelligence performer, UnityAction<BaseInteraction> onInteractionComplete)
    {
        // Add the performer to the list of performers
        performers.Add(new PerformerInformation()
        {
            performingAIIntelligence = performer,
            elapsedTime = 0,
            onInteractionComplete = onInteractionComplete
        });
    }

    private void Update()
    {
        // Check if the interaction type is over time
        if (interactionType != InteractionType.OverTime && performers.Count <= 0)
        {
            return;
        }

        // Update the interaction time for each performer
        for (int i = 0; i < performers.Count; i++)
        {
            PerformerInformation performer = performers[i]; // Get the performer

            float previousElapsedTime = performer.elapsedTime; // Get the previous interaction time
            performer.elapsedTime = Mathf.Min(performer.elapsedTime + Time.deltaTime, interactionDuration); // Update the interaction time
            float needChangePercentage = (performer.elapsedTime - previousElapsedTime) / interactionDuration; // Get the percentage of the interaction that has been completed

            if (needsChanges.Count > 0)
            {
                ApplyNeedsChanges(performer.performingAIIntelligence, needChangePercentage); // Apply the needs changes (if any)
            }

            // Check if the interaction time has reached the interaction duration
            if (performer.elapsedTime >= interactionDuration)
            {
                performer.onInteractionComplete.Invoke(this); // Invoke the on interaction complete event
                performers.RemoveAt(i); // Remove the performer from the list
                i--; // Decrement the index
            }
        }
    }

    /// <summary>
    /// Updates the current amount of people performing the interaction.
    /// </summary>
    public override void CompleteInteraction()
    {
        _currentInteractions--;
    }

    /// <summary>
    /// Information about a performer.
    /// </summary>
    public class PerformerInformation
    {
        public BaseCharacterIntelligence performingAIIntelligence;
        public float elapsedTime;
        public UnityAction<BaseInteraction> onInteractionComplete;
    }
}
