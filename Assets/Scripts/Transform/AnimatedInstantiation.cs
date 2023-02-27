using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedInstantiation : MonoBehaviour
{
    private Vector3 _targetScale; // The target scale of the object
    private float _animationTime = 0.5f; // The duration of the animation
    private float _fallSpeed = 2f; // The speed at which the object falls down

    private Vector3 _startScale; // The starting scale of the object
    private Vector3 _startPosition; // The starting position of the object
    private Vector3 _endPosition; // The end position of the object

    private void Start()
    {
        _targetScale = transform.localScale;
        _startScale = Vector3.zero;
        _startPosition = new Vector3(transform.position.x, transform.position.y - _fallSpeed, transform.position.z);

        StartCoroutine(AnimateObject());
    }

    // Call this method to start the animation
    public void StartAnimation()
    {
        StartCoroutine(AnimateObject());
    }

    private IEnumerator AnimateObject()
    {
        float timeElapsed = 0f;
        while (timeElapsed < _animationTime)
        {
            float t = timeElapsed / _animationTime;
            transform.localScale = Vector3.Lerp(_startScale, _targetScale, t);
            transform.position = _startPosition + new Vector3(0f, _fallSpeed * t, 0f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure we end with the correct scale and position
        transform.localScale = _targetScale;
        transform.position = _startPosition + new Vector3(0f, _fallSpeed, 0f);

        Destroy(this);
    }
}
