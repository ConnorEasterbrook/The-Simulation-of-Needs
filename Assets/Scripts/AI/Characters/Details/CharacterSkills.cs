using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSkills
{
    public int skillLevel = 10;
    public float monthlySalary = 100;

    private int traitPoints = 5;
    private List<Trait> traits = new List<Trait>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetSalary()
    {
        return monthlySalary;
    }

    public List<Trait> GenerateTraits(AutonomousIntelligence performer)
    {
        Debug.Log("Generating traits");
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
            Debug.Log(performerTraits.traits[randomTrait].name);

            // Check if a trait with the same id already exists
            for (int j = 0; j < traits.Count; j++)
            {
                if (traits[j].id == performerTraits.traits[randomTrait].id)
                {
                    i--;
                    continue;
                }
            }

            if (performerTraits.traits[randomTrait].value > traitPoints)
            {
                i--;
                continue;
            }

            traitPoints -= (int)performerTraits.traits[randomTrait].value;

            // Add the trait to the list
            Trait newTrait = new Trait();
            newTrait.id = performerTraits.traits[randomTrait].id;
            newTrait.name = performerTraits.traits[randomTrait].name;
            newTrait.description = performerTraits.traits[randomTrait].description;
            newTrait.change = performerTraits.traits[randomTrait].change;
            newTrait.value = performerTraits.traits[randomTrait].value;

            traits.Add(newTrait);
        }

        // CharacterNeedsUI characterNeedsUI = GameVariableConnector.instance.generalGUIManagerScript._characterNeedsUIScript;
        // characterNeedsUI.AddPerformer(performer.GetComponent<AutonomousIntelligence>());

        return traits;
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
    public float change;
    public float value;
}
