using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool invertY = false;

    // Update is called once per frame
    void Update()
    {
        if (invertY)
        {
            Quaternion flipY = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.LookRotation(_target.position - transform.position) * flipY;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
        }
    }
}
