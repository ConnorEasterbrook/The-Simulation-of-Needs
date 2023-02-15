using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communication
{
    public Dictionary<CommunicationKey, int> intValues = new Dictionary<CommunicationKey, int>();
    public Dictionary<CommunicationKey, float> floatValues = new Dictionary<CommunicationKey, float>();
    public Dictionary<CommunicationKey, bool> boolValues = new Dictionary<CommunicationKey, bool>();
    public Dictionary<CommunicationKey, string> stringValues = new Dictionary<CommunicationKey, string>();
    public Dictionary<CommunicationKey, Vector3> vector3Values = new Dictionary<CommunicationKey, Vector3>();
    public Dictionary<CommunicationKey, GameObject> GameObjectValues = new Dictionary<CommunicationKey, GameObject>();

    public void Set<T>(Dictionary<CommunicationKey, T> dictionary, CommunicationKey key, T value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public T Get<T>(Dictionary<CommunicationKey, T> dictionary, CommunicationKey key)
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            return default(T);
        }
    }

    public bool TryGet<T>(Dictionary<CommunicationKey, T> dictionary, CommunicationKey key, out T value)
    {
        if (dictionary.ContainsKey(key))
        {
            value = dictionary[key];
            return true;
        }
        else
        {
            value = default(T);
            return false;
        }
    }
}

/// <summary>
/// This enum contains all the keys for the communication dictionary.
/// </summary>
public enum CommunicationKey
{
    /// Character
    // Character Needs
    Character_Need_Hunger,
    Character_Need_Energy,
    Character_Need_Hygiene,

    // Character Max Needs
    Character_MaxNeed_Hunger,
    Character_MaxNeed_Energy,
    Character_MaxNeed_Hygiene,

    // Character Level
    Character_Level_Programming,

    /// Tasks
    // Task worker count
    Task_Worker_Count,
}
