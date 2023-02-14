using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores the needs of a character and handles the logic for updating them.
/// </summary>
public class CharacterNeeds
{
    private Communication _communication; // The communication object for this character

    // The needs of the character
    public float hunger = 100f; // The hunger level of the character
    public float energy = 100f; // The energy level of the character
    public float hygiene = 100f; // The happiness level of the character

    // The rate at which the needs decay
    public float hungerDecayRate = 1f; // The rate at which hunger decays
    public float energyDecayRate = 1f; // The rate at which energy decays
    public float hygieneDecayRate = 1f; // The rate at which hygiene decays

    // The cap for the needs
    public float hungerCap = 100f; // The cap for hunger
    public float energyCap = 100f; // The cap for energy
    public float hygieneCap = 100f; // The cap for hygiene

    public void Initialize(Communication communication)
    {
        _communication = communication;
    }

    public void UpdateNeeds()
    {
        hunger -= hungerDecayRate * Time.deltaTime;
        // _communication.Set<float>(_communication.floatValues, CommunicationKey.Character_Need_Hunger, hunger); // Set the hunger need

        energy -= energyDecayRate * Time.deltaTime;
        // _communication.Set<float>(_communication.floatValues, CommunicationKey.Character_Need_Energy, energy); // Set the energy need

        hygiene -= hygieneDecayRate * Time.deltaTime;
        // _communication.Set<float>(_communication.floatValues, CommunicationKey.Character_Need_Hygiene, hygiene); // Set the hygiene need

        hunger = Mathf.Clamp(hunger, 0f, hungerCap);
        energy = Mathf.Clamp(energy, 0f, energyCap);
        hygiene = Mathf.Clamp(hygiene, 0f, hygieneCap);
    }

    public void UpdateIndividualNeed(NeedType needType, float value)
    {
        switch (needType)
        {
            case NeedType.Hunger:
                hunger += value;
                hunger = Mathf.Clamp(hunger, 0f, hungerCap);
                break;
                
            case NeedType.Energy:
                energy += value;
                energy = Mathf.Clamp(energy, 0f, energyCap);
                break;

            case NeedType.Hygiene:
                hygiene += value;
                hygiene = Mathf.Clamp(hygiene, 0f, hygieneCap);
                break;

            default:
                Debug.LogError("Invalid NeedType");
                break;
        }
    }

    public float GetNeedValue(NeedType needType)
    {
        switch (needType)
        {
            case NeedType.Hunger:
                return hunger;

            case NeedType.Energy:
                return energy;

            case NeedType.Hygiene:
                return hygiene;
                
            default:
                Debug.LogError("Invalid NeedType");
                return 0f;
        }
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
