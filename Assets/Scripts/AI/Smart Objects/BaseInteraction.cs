using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteraction : MonoBehaviour
{
    public string displayName;
    public InteractionType interactionType = InteractionType.Instant;
    public float interactionDuration = 1f;

    public abstract bool CanPerformInteraction();
    public abstract void PerformInteraction(GameObject performer, UnityEvent<BaseInteraction> onInteractionComplete);
    public abstract void CancelInteraction();
}

public enum InteractionType
{
    Instant,
    OverTime
}
