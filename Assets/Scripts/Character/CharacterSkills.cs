using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour
{
    // [SerializeField] private int _programmingSkill;
    // [SerializeField] private GameVariableConnector _gameVariableConnectorScript;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(IncreaseProjectProgress());
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (_gameVariableConnectorScript.GetProgressOnBarScript().CheckIfProjectIsFinished())
    //     {
    //         StopCoroutine(IncreaseProjectProgress());
    //     }
    // }

    // private IEnumerator IncreaseProjectProgress()
    // {
    //     while (true)
    //     {
    //         _gameVariableConnectorScript.GetProgressOnBarScript().IncreaseProgress(_programmingSkill, _programmingSkill);
    //         yield return new WaitForSeconds(1f);
    //     }
    // }
}
