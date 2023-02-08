using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressOnBar : MonoBehaviour
{
    public static float progress;
    private float _targetValue;
    private float _increaseSpeed;

    // Update is called once per frame
    void Update()
    {
        progress = Mathf.Lerp(progress, _targetValue, Time.deltaTime * _increaseSpeed);
        progress = Mathf.Clamp(progress, 0, 100);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int random = Random.Range(1, 10);
            IncreaseProgress(20f, random);
        }

        GetComponent<Slider>().value = progress;
    }

    public void IncreaseProgress(float percentageAmount, float increaseSpeed)
    {
        _targetValue = progress + percentageAmount;
        _increaseSpeed = increaseSpeed;
    }
}
