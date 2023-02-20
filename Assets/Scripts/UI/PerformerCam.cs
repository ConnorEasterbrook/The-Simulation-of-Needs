using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformerCam
{
    private Camera _camera;
    private RenderTexture _renderTexture;

    public PerformerCam(GameObject performer, RenderTexture renderTexture)
    {
        _camera = performer.transform.GetChild(0).GetComponent<Camera>();
        _renderTexture = renderTexture;

        _camera.targetTexture = _renderTexture;
    }

    public void ChangeCamera(GameObject performer)
    {
        _camera.gameObject.SetActive(false);
        _camera = performer.transform.GetChild(0).GetComponent<Camera>();
        _camera.gameObject.SetActive(true);
        _camera.targetTexture = _renderTexture;
    }
}
