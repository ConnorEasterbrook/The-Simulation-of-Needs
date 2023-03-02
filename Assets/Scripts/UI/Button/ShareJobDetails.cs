using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShareJobDetails : MonoBehaviour
{
    [SerializeField] private TMP_InputField _taskName;
    [SerializeField] private TMP_Dropdown _projectType, _projectLanguage, _projectComplexity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        CreateJob.instance.SetTaskName(_taskName, _projectType, _projectLanguage, _projectComplexity);
    }
}
