using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : GridBuildCore
{
    /// <summary>
    /// Controls the building of the object
    /// </summary>
    public void BuildObject()
    {
        if (objectPrefab == null)
        {
            return;
        }

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
            objectPrefab.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            objectPrefab.transform.localScale = _initialObjectScale;
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
            _length = Mathf.Max(_length, tileSize);

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
        int numTiles = Mathf.FloorToInt(_length / tileSize);
        float remainingLength = _length - (numTiles * tileSize);

        if (numTiles == 0)
        {
            return;
        }

        for (int i = 0; i < numTiles; i++)
        {
            Vector3 position = _startPoint + _direction * (tileSize * 0.5f + tileSize * i);

            GameObject newObject = MonoBehaviour.Instantiate(objectPrefab, position, Quaternion.LookRotation(_direction), plane.transform);
            newObject.AddComponent<BoxCollider>();
            newObject.transform.localScale = new Vector3(_initialObjectScale.x, _initialObjectScale.y, tileSize);
            newObject.tag = newObject.name = "Wall";
        }

        if (remainingLength == 0)
        {
            return;
        }

        Vector3 lastPosition = _startPoint + _direction * (tileSize * numTiles + remainingLength * 0.5f);
        GameObject newObjectLast = MonoBehaviour.Instantiate(objectPrefab, lastPosition, Quaternion.LookRotation(_direction), plane.transform);
        newObjectLast.AddComponent<BoxCollider>();
        newObjectLast.transform.localScale = new Vector3(_initialObjectScale.x, _initialObjectScale.y, remainingLength);
        newObjectLast.tag = newObjectLast.name = "Wall";
    }
}
