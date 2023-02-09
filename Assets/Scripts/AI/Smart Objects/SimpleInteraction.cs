using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleInteraction : BaseInteraction
{
    public int maxSimultaneousInteractions = 1;
    public List<PerformerInformation> performers = new List<PerformerInformation>();
    private int _currentInteractions = 0;

    public override bool CanPerformInteraction()
    {
        return _currentInteractions < maxSimultaneousInteractions;
    }

    public override void PerformInteraction(GameObject performer, UnityEvent<BaseInteraction> onInteractionComplete)
    {
        if (CanPerformInteraction())
        {
            _currentInteractions++;
        }

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

    public override void CancelInteraction()
    {
        throw new System.NotImplementedException();
    }

    private void InstantInteraction(GameObject performer, UnityEvent<BaseInteraction> onInteractionComplete)
    {
        if (onInteractionComplete != null)
        {
            onInteractionComplete.Invoke(this);
        }
    }

    private void OverTimeInteraction(GameObject performer, UnityEvent<BaseInteraction> onInteractionComplete)
    {
        performers.Add(new PerformerInformation()
        {
            interactionTime = interactionDuration,
            onInteractionComplete = onInteractionComplete
        });
    }

    private void Update()
    {
        for (int i = 0; i < performers.Count; i++)
        {
            performers[i].interactionTime += Time.deltaTime;

            if (performers[i].interactionTime >= interactionDuration)
            {
                performers[i].onInteractionComplete.Invoke(this);
                performers.RemoveAt(i);
                i--;
            }
        }
    }

    public class PerformerInformation
    {
        public float interactionTime;
        public UnityEvent<BaseInteraction> onInteractionComplete;
    }
}
