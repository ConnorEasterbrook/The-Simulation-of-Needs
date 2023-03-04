using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColour : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

        transform.localScale = new Vector3(Random.Range(0.25f, .75f), Random.Range(.5f, 1.25f), Random.Range(0.25f, .75f));
    }
}
