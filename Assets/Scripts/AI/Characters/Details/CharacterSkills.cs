using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSkills
{
    public float skillIncreasePercentage = 0f;
    public int skillLevel = 10;
    public float monthlySalary = 100;

    private int traitPoints = 5;
    private List<Trait> traits = new List<Trait>();

    public float GetSalary()
    {
        return monthlySalary;
    }

    public List<Trait> GenerateTraits(AutonomousIntelligence performer)
    {
        TextAsset traitsJson = new TextAsset("Hello");
        traitsJson = GameVariableConnector.instance.GetTraitsJson();

        if (traitsJson == null)
        {
            Debug.LogError("No traits.json file found!");
        }

        // PerformerTrait performerTraits = JsonUtility.FromJson<PerformerTrait>(traitsJson.text);
        var performerTraits = JsonUtility.FromJson<CharacterTraits>(traitsJson.text);

        for (int i = 0; i < 3; i++)
        {
            // Get a random trait and subtract its value from the trait points
            int randomTrait = Random.Range(0, performerTraits.traits.Count);

            traitPoints -= (int)performerTraits.traits[randomTrait].value;

            // Add the trait to the list
            Trait newTrait = new Trait();
            newTrait.id = performerTraits.traits[randomTrait].id;
            newTrait.name = performerTraits.traits[randomTrait].name;
            newTrait.description = performerTraits.traits[randomTrait].description;
            newTrait.type = performerTraits.traits[randomTrait].type;
            newTrait.change = performerTraits.traits[randomTrait].change;
            newTrait.value = performerTraits.traits[randomTrait].value;

            traits.Add(newTrait);

            // Remove the trait from the list
            performerTraits.traits.RemoveAt(randomTrait);

            // Remove trait with matching id
            for (int j = 0; j < performerTraits.traits.Count; j++)
            {
                if (performerTraits.traits[j].id == newTrait.id)
                {
                    performerTraits.traits.RemoveAt(j);
                }
            }
        }

        TraitEffect(performer);

        skillLevel = Random.Range(5, 40);
        GenerateSalary();

        return traits;
    }

    private void TraitEffect(AutonomousIntelligence performer)
    {
        for (int i = 0; i < traits.Count; i++)
        {
            string type = traits[i].type;

            switch (type)
            {
                case "threshold":
                    performer.characterNeedsScript.ChangeThreshold(traits[i].change);
                    break;

                case "efficiency":
                    skillLevel += (int)traits[i].change;
                    break;

                case "hungerCap":
                    performer.characterNeedsScript.hungerCap += traits[i].change;
                    break;

                case "energyCap":
                    performer.characterNeedsScript.energyCap += traits[i].change;
                    break;

                case "hygieneCap":
                    performer.characterNeedsScript.hygieneCap += traits[i].change;
                    break;

                default:
                    Debug.LogError("Trait type not found! Type: " + type + "");
                    break;
            }
        }
    }

    private void GenerateSalary()
    {
        monthlySalary = skillLevel * 50;
    }

    public void IncreaseSkillLevel()
    {
        skillIncreasePercentage += Random.Range(0f, 0.05f);

        if (skillIncreasePercentage >= 100f)
        {
            skillIncreasePercentage = 0f;
            skillLevel++;
        }
    }
}

[System.Serializable]
public class CharacterTraits
{
    public List<Trait> traits = new List<Trait>();
}

[System.Serializable]
public class Trait
{
    public float id;
    public string name;
    public string description;
    public string type;
    public float change;
    public float value;
}
