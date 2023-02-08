using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariableConnector : MonoBehaviour
{
    public ProgressOnBar _progressOnBarScript;

    public ProgressOnBar GetProgressOnBarScript()
    {
        return _progressOnBarScript;
    }
}
