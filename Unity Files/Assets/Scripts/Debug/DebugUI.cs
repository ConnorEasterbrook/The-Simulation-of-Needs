using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private GameObject _performer;
    [SerializeField] private GameObject _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPerformer()
    {
        Instantiate(_performer, _spawnPoint.transform.position, Quaternion.identity);
    }
}
