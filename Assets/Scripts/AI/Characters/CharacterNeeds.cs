using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores the needs of a character and handles the logic for updating them.
/// </summary>
public class CharacterNeeds
{
    // The needs of the character
    public float hunger = 100f; // The hunger level of the character
    public float energy = 100f; // The energy level of the character
    public float hygiene = 100f; // The happiness level of the character

    // The rate at which the needs decay
    public float hungerDecayRate = 0.1f; // The rate at which hunger decays
    public float energyDecayRate = 0.1f; // The rate at which energy decays
    public float hygieneDecayRate = 0.1f; // The rate at which hygiene decays

    // The rate at which the needs increase
    public float hungerIncreaseRate = 0.2f; // The rate at which hunger increases
    public float energyIncreaseRate = 0.2f; // The rate at which energy increases
    public float hygieneIncreaseRate = 0.2f; // The rate at which hygiene increases

    // The cap for the needs
    public float hungerCap = 100f; // The cap for hunger
    public float energyCap = 100f; // The cap for energy
    public float hygieneCap = 100f; // The cap for hygiene

    public void UpdateNeeds()
    {
        hunger -= hungerDecayRate * Time.deltaTime;
        energy -= energyDecayRate * Time.deltaTime;
        hygiene -= hygieneDecayRate * Time.deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, hungerCap);
        energy = Mathf.Clamp(energy, 0f, energyCap);
        hygiene = Mathf.Clamp(hygiene, 0f, hygieneCap);
    }
}

/// <summary>
/// This class stores the needs of a character and handles the logic for updating them.
/// </summary>
public enum NeedType
{
    Hunger,
    Energy,
    Hygiene
}
