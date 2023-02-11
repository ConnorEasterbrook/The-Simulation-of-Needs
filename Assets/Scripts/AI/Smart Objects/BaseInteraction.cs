using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <Summary>
/// The base class for interactions. All interactions must inherit from this class.
/// </Summary>
public abstract class BaseInteraction : MonoBehaviour
{
    public string displayName; // The name of the interaction
    public InteractionType interactionType = InteractionType.Instant; // The type of interaction
    public float interactionDuration = 1f; // The duration of the interaction
    public List<InteractionNeedsChange> needsChanges = new List<InteractionNeedsChange>(); // The list of needs that will be changed by the interaction

    public abstract bool CanPerformInteraction(); // Returns true if the interaction can be performed
    public abstract void PerformInteraction(BaseCharacterIntelligence performer, UnityAction<BaseInteraction> onInteractionComplete); // Performs the interaction
    public abstract void CancelInteraction(); // Cancels the interaction

    public void ApplyNeedsChanges(BaseCharacterIntelligence performerIntelligence, float percentage)
    {
        foreach (var needsChange in needsChanges)
        {
            // Debug.Log($"Applying {needsChange.changeAmount * percentage} to {needsChange.targetNeedType}");
            // Update performer need through BaseCharacterIntelligence.cs 
            performerIntelligence.UpdateIndividualNeed(needsChange.targetNeedType, needsChange.changeAmount * percentage);
        }
    }
}

/// <summary>
/// The type of interaction.
/// </summary>
public enum InteractionType
{
    Instant,
    OverTime
}

/// <summary>
/// Get the changing needs of the interaction.
/// </summary>
[System.Serializable]
public class InteractionNeedsChange
{
    public NeedType targetNeedType; // The type of need that will be changed
    public float changeAmount; // The amount that the need will be changed by
}
