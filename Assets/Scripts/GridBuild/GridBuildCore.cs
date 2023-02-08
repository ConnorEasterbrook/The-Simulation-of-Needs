using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildCore : MonoBehaviour
{
    public static bool isBuilding = true;
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private GameObject _plane;
    private Plane _gridPlane;
    [SerializeField] private float _gridSize = 1f;

    private GameObject _currentObject;
    [SerializeField] private GameObject _previewObject;
    private bool _isBuilding;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _objectPosition;
    private Vector3 _initialObjectScale;
    private Vector3 _objectScale;

    // Start is called before the first frame update
    void Start()
    {
        _gridPlane = new Plane(_plane.transform.up, _plane.transform.position);
        _initialObjectScale = _objectPrefab.transform.localScale;
        _objectScale = _objectPrefab.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            BuildObject();
        }
    }

    private void BuildObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (_gridPlane.Raycast(ray, out float distance))
            {
                _startPoint = SnapToGrid(ray.GetPoint(distance));
                _previewObject.transform.position = _startPoint;
            }
        }
        else if (Input.GetMouseButton(0) && _previewObject.activeSelf)
        {
            if (_gridPlane.Raycast(ray, out float distance))
            {
                _endPoint = SnapToGrid(ray.GetPoint(distance));
                UpdatePreviewObject();
            }
        }
        else if (Input.GetMouseButtonUp(0) && _previewObject.activeSelf)
        {
            InstantiateObject();
            _previewObject.transform.localScale = _initialObjectScale;
        }
        else
        {
            if (_gridPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 snappedPoint = SnapToGrid(hitPoint);

                _objectPrefab.transform.position = new Vector3(snappedPoint.x, snappedPoint.y + (_objectScale.y * 0.5f), snappedPoint.z);
                _objectPrefab.transform.localScale = _objectScale;
            }
        }
    }

    private void UpdatePreviewObject()
    {
        Vector3 direction = _endPoint - _startPoint;
        float length = direction.magnitude;
        direction.Normalize();

        if (SnapToGrid(_startPoint) != SnapToGrid(_endPoint))
        {
            length = Mathf.Max(length, _gridSize);

            _previewObject.transform.localScale = new Vector3(_objectScale.x, _objectScale.y, length);
            _previewObject.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            _previewObject.transform.localScale = _initialObjectScale;
        }

        _objectPosition = _startPoint + direction * length * 0.5f;
        _objectPosition.y += _objectScale.y * 0.5f;
        _previewObject.transform.position = _objectPosition;
    }

    private void InstantiateObject()
    {
        GameObject newObject = Instantiate(_objectPrefab, _objectPosition, Quaternion.identity, _plane.transform);
        newObject.SetActive(true);
        newObject.transform.rotation = Quaternion.LookRotation(_endPoint - _startPoint);

        newObject.transform.localScale = new Vector3(_objectScale.x, _objectScale.y, _previewObject.transform.localScale.z);
    }

    private Vector3 SnapToGrid(Vector3 hitPoint)
    {
        float x = Mathf.Round(hitPoint.x / _gridSize) * _gridSize;
        float y = Mathf.Round(hitPoint.y / _gridSize) * _gridSize;
        float z = Mathf.Round(hitPoint.z / _gridSize) * _gridSize;

        return new Vector3(x, y, z);
    }

    public void SetBuildingObject(GameObject buildingObject)
    {
        _objectPrefab = buildingObject;
        _objectScale = _objectPrefab.transform.localScale;
        _previewObject.SetActive(true);
    }
}
