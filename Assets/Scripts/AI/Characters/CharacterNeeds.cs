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
    // public float hunger { get { return _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Hunger); } set { _communication.Set<float>(_communication.floatValues, CommunicationKey.Character_Need_Hunger, value); } } // The hunger level of the character
    // public float energy { get { return _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Energy); } set { _communication.Set<float>(_communication.floatValues, CommunicationKey.Character_Need_Energy, value); } } // The energy level of the character
    // public float hygiene { get { return _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Hygiene); } set { _communication.Set<float>(_communication.floatValues, CommunicationKey.Character_Need_Hygiene, value); } } // The hygiene level of the character
    public float hunger; // The hunger level of the character
    public float energy; // The energy level of the character
    public float hygiene; // The hygiene level of the character

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

        hunger = hungerCap;
        energy = energyCap;
        hygiene = hygieneCap;
    }

    public void UpdateNeeds()
    {
        hunger -= hungerDecayRate * Time.deltaTime;
        energy -= energyDecayRate * Time.deltaTime;
        hygiene -= hygieneDecayRate * Time.deltaTime;

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

    public bool AreNeedsFine()
    {
        if (hunger < 60f || energy <= 60f || hygiene <= 60f)
        {
            return false;
        }
        else
        {
            return true;
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
