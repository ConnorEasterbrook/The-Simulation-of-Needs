using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleInteraction : BaseInteraction
{
    public int maxSimultaneousInteractions = 1; // The maximum amount of people that can perform the interaction at the same time
    public List<PerformerInformation> performers = new List<PerformerInformation>(); // The list of performers
    private int _currentInteractions = 0; // The current amount of people performing the interaction

    /// <summary>
    /// Returns true if the interaction can be performed.
    /// </summary>
    public override bool CanPerformInteraction()
    {
        return _currentInteractions < maxSimultaneousInteractions;
    }

    /// <summary>
    /// Performs the interaction.
    /// </summary>
    public override void PerformInteraction(GameObject performer, UnityAction<BaseInteraction> onInteractionComplete)
    {
        // Check if the interaction can be performed
        if (CanPerformInteraction())
        {
            _currentInteractions++; // Increment the current amount of peorple performing the interaction
        }

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
    private void InstantInteraction(GameObject performer, UnityAction<BaseInteraction> onInteractionComplete)
    {
        // Perform the interaction
        if (onInteractionComplete != null)
        {
            onInteractionComplete.Invoke(this);
        }
    }

    /// <summary>
    /// Performs the interaction over time.
    /// </summary>
    private void OverTimeInteraction(GameObject performer, UnityAction<BaseInteraction> onInteractionComplete)
    {
        // Add the performer to the list of performers
        performers.Add(new PerformerInformation()
        {
            interactionTime = interactionDuration,
            onInteractionComplete = onInteractionComplete
        });
    }

    private void Update()
    {
        // Update the interaction time for each performer
        for (int i = 0; i < performers.Count; i++)
        {
            performers[i].interactionTime += Time.deltaTime; // Increment the interaction time

            // Check if the interaction time has reached the interaction duration
            if (performers[i].interactionTime >= interactionDuration)
            {
                performers[i].onInteractionComplete.Invoke(this); // Invoke the on interaction complete event
                performers.RemoveAt(i); // Remove the performer from the list
                i--; // Decrement the index
            }
        }
    }

    /// <summary>
    /// Information about a performer.
    /// </summary>
    public class PerformerInformation
    {
        public float interactionTime;
        public UnityAction<BaseInteraction> onInteractionComplete;
    }
}
