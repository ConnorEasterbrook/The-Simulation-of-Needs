using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A more complex intelligence that picks interactions based on needs and performs it. Best used for Autonomous Agents like Sims.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AutonomousIntelligence : BaseCharacterIntelligence
{
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>(); // Get the navmesh agent component
        _characterNeedsUIScript.Initialize(_characterNeedsScript); // Initialize the character needs UI;
    }

    // public override void Update()
    // {
    //     base.Update();
    // }
}
