using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _movementSmoothTime = 0.1f;

    [Header("Camera Rotation")]
    [SerializeField] private float _mouseSensitivity = 5f;
    [SerializeField] private float _cameraSmoothTime = 0.1f;
    [SerializeField] private Vector2 _xRotationClamp = new Vector2(-90f, 90f);
    private float _xRotation;
    private float _yRotation;
    private Vector3 _rotation;

    [Header("Camera Zoom")]
    [SerializeField] private float _minCameraHeight = 1f;
    [SerializeField] private float _maxCameraHeight = 20f;
    [SerializeField] private float _zoomSpeed = 5f;
    private float currentCameraHeight;

    private GameVariableConnector _gameVariableConnector;

    private void Start()
    {
        _movementSpeed *= 10f;

        _rotation = transform.eulerAngles;
        _xRotation = _rotation.x;
        _yRotation = _rotation.y;

        currentCameraHeight = transform.position.y;

        _gameVariableConnector = GameVariableConnector.instance;
    }

    private void Update()
    {
        if (!_gameVariableConnector._generalGUIManagerScript.stopMovement)
        {
            if (Input.GetMouseButton(1))
            {
                RotateCamera();
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                SetCameraHeight(scroll);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = MoveCamera();
        newPosition.y = currentCameraHeight;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _zoomSpeed);

        transform.eulerAngles = _rotation;
    }

    private Vector3 MoveCamera()
    {
        if (!_gameVariableConnector._generalGUIManagerScript.stopMovement)
        {
            Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveDirection = transform.rotation * moveDirection;
            Vector3 velocity = Vector3.zero;
            Vector3 returnVector = Vector3.SmoothDamp(transform.position, transform.position + moveDirection * _movementSpeed, ref velocity, _movementSmoothTime);
            return returnVector;
        }
        else
        {
            return transform.position;
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _xRotation += -mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _xRotationClamp.x, _xRotationClamp.y);

        _yRotation += mouseX;

        _rotation = new Vector3(_xRotation, _yRotation, 0f);

    }

    public void SetCameraHeight(float scroll)
    {
        currentCameraHeight -= scroll * _zoomSpeed;
        currentCameraHeight = Mathf.Clamp(currentCameraHeight, _minCameraHeight, _maxCameraHeight);
    }
}
