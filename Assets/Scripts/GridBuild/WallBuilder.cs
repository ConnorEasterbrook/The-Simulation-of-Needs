using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder
{
    public GameObject _objectPrefab;
    public GameObject _plane;
    public Plane _gridPlane;
    public float _tileSize = 1f;

    public GridBuildCore _gridBuildCore;
    private GameObject _previewObject;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _objectPosition;
    private Vector3 _initialObjectScale;

    private Vector3 _direction;
    private float _length;

    public WallBuilder(GridBuildCore gridBuildCore)
    {
        _gridBuildCore = gridBuildCore;
        _objectPrefab = _gridBuildCore.objectPrefab;
        _previewObject = _gridBuildCore.objectPrefab;
        _plane = _gridBuildCore.plane;
        _gridPlane = _gridBuildCore.gridPlane;
        _tileSize = _gridBuildCore.tileSize;
        _initialObjectScale = _gridBuildCore.initialObjectScale;
    }

    /// <summary>
    /// Controls the building of the object
    /// </summary>
    public void BuildObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            _previewObject.transform.position = _startPoint;
        }
        else if (Input.GetMouseButton(0) && _previewObject.activeSelf)
        {
            _endPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            PreviewObject();
        }
        else if (Input.GetMouseButtonUp(0) && _previewObject.activeSelf)
        {
            _endPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            InstantiateObject();

            _previewObject.transform.localScale = _initialObjectScale; // Reset scale
        }
        else
        {
            Vector3 hitPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            _objectPrefab.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            _objectPrefab.transform.localScale = _initialObjectScale;
        }
    }

    /// <summary>
    /// Previews the object
    /// </summary>
    private void PreviewObject()
    {
        _direction = _endPoint - _startPoint;
        _length = _direction.magnitude;
        _direction.Normalize();

        if (SnapToGrid(_startPoint) != SnapToGrid(_endPoint))
        {
            _length = Mathf.Max(_length, _tileSize);

            _previewObject.transform.localScale = new Vector3(_initialObjectScale.x, _initialObjectScale.y, _length);
            _previewObject.transform.rotation = Quaternion.LookRotation(_direction);
        }
        else
        {
            _previewObject.transform.localScale = _initialObjectScale;
        }

        _objectPosition = _startPoint + _direction * _length * 0.5f;
        _previewObject.transform.position = _objectPosition;
    }

    /// <summary>
    /// Instantiates the object
    /// </summary>
    private void InstantiateObject()
    {
        int numTiles = Mathf.FloorToInt(_length / _tileSize);
        float remainingLength = _length - (numTiles * _tileSize);

        if (numTiles == 0)
        {
            return;
        }

        for (int i = 0; i < numTiles; i++)  
        {
            Vector3 position = _startPoint + _direction * (_tileSize * 0.5f + _tileSize * i);

            GameObject newObject = MonoBehaviour.Instantiate(_objectPrefab, position, Quaternion.LookRotation(_direction), _plane.transform);
            newObject.AddComponent<BoxCollider>();
            newObject.transform.localScale = new Vector3(_initialObjectScale.x, _initialObjectScale.y, _tileSize);
            newObject.tag = newObject.name = "Wall";
        }

        if (remainingLength == 0)
        {
            return;
        }

        Vector3 lastPosition = _startPoint + _direction * (_tileSize * numTiles + remainingLength * 0.5f);
        GameObject newObjectLast = MonoBehaviour.Instantiate(_objectPrefab, lastPosition, Quaternion.LookRotation(_direction), _plane.transform);
        newObjectLast.AddComponent<BoxCollider>();
        newObjectLast.transform.localScale = new Vector3(_initialObjectScale.x, _initialObjectScale.y, remainingLength);
        newObjectLast.tag = newObjectLast.name = "Wall";
    }

    /// <summary>
    /// Gets the mouse position on the grid plane
    /// </summary>
    public Vector3 GetMouseWorldPositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (_gridPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Snaps the given point to the grid
    /// </summary>
    public Vector3 SnapToGrid(Vector3 hitPoint)
    {
        float x = Mathf.Round(hitPoint.x / _tileSize) * _tileSize;
        float y = hitPoint.y + (_initialObjectScale.y * 0.5f);
        float z = Mathf.Round(hitPoint.z / _tileSize) * _tileSize;

        return new Vector3(x, y, z);
    }
}
