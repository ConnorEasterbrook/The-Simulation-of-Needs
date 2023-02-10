using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteraction : MonoBehaviour
{
    public string displayName; // The name of the interaction
    public InteractionType interactionType = InteractionType.Instant; // The type of interaction
    public float interactionDuration = 1f; // The duration of the interaction

    public abstract bool CanPerformInteraction(); // Returns true if the interaction can be performed
    public abstract void PerformInteraction(GameObject performer, UnityAction<BaseInteraction> onInteractionComplete); // Performs the interaction
    public abstract void CancelInteraction(); // Cancels the interaction
}

/// <summary>
/// The type of interaction.
/// </summary>
public enum InteractionType
{
    Instant,
    OverTime
}
