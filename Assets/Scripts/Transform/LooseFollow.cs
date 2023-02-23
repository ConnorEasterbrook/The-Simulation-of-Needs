using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseFollow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    private Vector3 currentTransform;

    // Start is called before the first frame update
    void Start()
    {
        currentTransform = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_target != null)
       {    
            // Bob up and down while lerping after the target
            currentTransform = Vector3.Lerp(currentTransform, _target.transform.position, 0.5f);
            currentTransform.y = Mathf.Sin(Time.time * 2) * 0.1f + _target.transform.position.y;
            transform.position = currentTransform;
            transform.rotation = _target.transform.rotation;

            // currentTransform = Vector3.Lerp(currentTransform, _target.transform.position, 0.1f);
        }
    }
}
