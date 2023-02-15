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
        performers.Add(new PerformerInformation()
        {
            performingAIIntelligence = performer,
            elapsedTime = 0,
            onInteractionComplete = onInteractionComplete
        });
    }

    /// <summary>
    /// Cancels the interaction.
    /// </summary>
    public override void CancelInteraction()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        // Check if the interaction type is over time
        if (interactionType != InteractionType.Work && performers.Count <= 0)
        {
            return;
        }

        // Update the interaction time for each performer
        for (int i = 0; i < performers.Count; i++)
        {
            PerformerInformation performer = performers[i]; // Get the performer

            AssignWorkFromType(performer);

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

    
}
