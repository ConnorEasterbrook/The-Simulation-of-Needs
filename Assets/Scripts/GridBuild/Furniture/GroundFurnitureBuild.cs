using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFurnitureBuild : GridBuildCore
{
    public void BuildObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            previewObject.transform.position = startPoint;
        }
        else if (Input.GetMouseButton(0) && previewObject.activeSelf)
        {
            endPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            PreviewObject();
        }
        else if (Input.GetMouseButtonUp(0) && previewObject.activeSelf)
        {
            endPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            InstantiateObject();

            previewObject.transform.localScale = initialObjectScale; // Reset scale
        }
        else
        {
            Vector3 hitPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
            objectPrefab.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            objectPrefab.transform.localScale = initialObjectScale;
        }
    }

    /// <summary>
    /// Previews the object
    /// </summary>
    private void PreviewObject()
    {
        _direction = endPoint - startPoint;
        _length = _direction.magnitude;
        _direction.Normalize();

        if (SnapToGrid(startPoint) != SnapToGrid(endPoint))
        {
            _length = Mathf.Max(_length, tileSize);

            previewObject.transform.localScale = new Vector3(initialObjectScale.x, initialObjectScale.y, _length);
            previewObject.transform.rotation = Quaternion.LookRotation(_direction);
        }
        else
        {
            previewObject.transform.localScale = initialObjectScale;
        }

        objectPosition = startPoint + _direction * _length * 0.5f;
        previewObject.transform.position = objectPosition;
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
            Vector3 position = startPoint + _direction * (tileSize * 0.5f + tileSize * i);

            GameObject newObject = MonoBehaviour.Instantiate(objectPrefab, position, Quaternion.LookRotation(_direction), plane.transform);
            newObject.AddComponent<BoxCollider>();
            newObject.transform.localScale = new Vector3(initialObjectScale.x, initialObjectScale.y, tileSize);
            newObject.tag = newObject.name = "Wall";
        }

        if (remainingLength == 0)
        {
            return;
        }

        Vector3 lastPosition = startPoint + _direction * (tileSize * numTiles + remainingLength * 0.5f);
        GameObject newObjectLast = MonoBehaviour.Instantiate(objectPrefab, lastPosition, Quaternion.LookRotation(_direction), plane.transform);
        newObjectLast.AddComponent<BoxCollider>();
        newObjectLast.transform.localScale = new Vector3(initialObjectScale.x, initialObjectScale.y, remainingLength);
        newObjectLast.tag = newObjectLast.name = "Wall";
    }


}
