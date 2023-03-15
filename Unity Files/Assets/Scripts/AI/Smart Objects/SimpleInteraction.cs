using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <Summary>
/// Simple interaction that can be performed by multiple people at the same time.
/// </Summary>
public class SimpleInteraction : BaseInteraction
{
    public List<PerformerInformation> performers = new List<PerformerInformation>(); // The list of performers

    /// <summary>
    /// Returns true if the interaction can be performed.
    /// </summary>
    public override bool CanPerformInteraction()
    {
        if (GameVariableConnector.instance.IsGamePaused())
        {
            return false;
        }

        if (_currentInteractions < maxSimultaneousInteractions)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    public override void CompleteInteraction(int characterID)
    {
        SmartObject smartObject = GetComponent<SmartObject>(); // Get the smart object

        // Loop through the interaction points to see what performer has completed their task and set the bool to false
        for (int i = 0; i < smartObject.interactionPoints.Length; i++)
        {
            if (smartObject.interactionPointOccupierID[i] == characterID)
            {
                smartObject.interactionPointsOccupied[i] = false;
                smartObject.interactionPointOccupierID[i] = -1;
                break;
            }
        }

        _currentInteractions--;
    }


}
