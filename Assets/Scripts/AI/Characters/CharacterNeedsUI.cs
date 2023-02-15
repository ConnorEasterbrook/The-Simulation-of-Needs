using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class stores the character's UI elements and handles the logic for updating them.
/// </summary>
[System.Serializable]
public class CharacterNeedsUI
{
    private Communication _communication; // The communication object for this character

    [SerializeField] private Slider _hungerSlider; // The slider for the hunger need
    [SerializeField] private Slider _energySlider; // The slider for the energy need
    [SerializeField] private Slider _hygieneSlider; // The slider for the hygiene need

    private CharacterNeeds _characterNeeds; // The character's needs

    public void Initialize(Communication communication, CharacterNeeds characterNeeds)
    {
        _communication = communication;
        _characterNeeds = characterNeeds;
    }

    // Start is called before the first frame update
    void Start()
    {
        _hungerSlider.maxValue = _characterNeeds.hungerCap;
        _energySlider.maxValue = _characterNeeds.energyCap;
        _hygieneSlider.maxValue = _characterNeeds.hygieneCap;

        _hungerSlider.value = _characterNeeds.hunger;
        _energySlider.value = _characterNeeds.energy;
        _hygieneSlider.value = _characterNeeds.hygiene;

        // _hungerSlider.value = _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Hunger);
        // _energySlider.value = _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Energy);
        // _hygieneSlider.value = _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Hygiene);
    }

    public void UpdateSliders()
    {
        // _hungerSlider.value = _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Hunger);
        // _energySlider.value = _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Energy);
        // _hygieneSlider.value = _communication.Get<float>(_communication.floatValues, CommunicationKey.Character_Need_Hygiene);

        _hungerSlider.value = _characterNeeds.hunger;
        _energySlider.value = _characterNeeds.energy;
        _hygieneSlider.value = _characterNeeds.hygiene;
    }
}
