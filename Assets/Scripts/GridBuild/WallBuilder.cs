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
            startPoint = SnapToGrid(GetMouseWorldPositionOnPlane(), true);
            previewObject.transform.position = startPoint;
        }
        else if (Input.GetMouseButton(0) && previewObject.activeSelf)
        {
            endPoint = SnapToGrid(GetMouseWorldPositionOnPlane(), true);
            PreviewObject();
        }
        else if (Input.GetMouseButtonUp(0) && previewObject.activeSelf)
        {
            endPoint = SnapToGrid(GetMouseWorldPositionOnPlane(), true);
            InstantiateObject();

            previewObject.transform.localScale = initialObjectScale; // Reset scale
        }
        else
        {
            Vector3 hitPoint = SnapToGrid(GetMouseWorldPositionOnPlane(), true);
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

        if (SnapToGrid(startPoint, true) != SnapToGrid(endPoint, true))
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

        if (numTiles == 0 || !validRaycast)
        {
            return;
        }

        for (int i = 0; i < numTiles; i++)
        {
            Vector3 position = startPoint + _direction * (tileSize * 0.5f + tileSize * i);

            GameObject newObject = MonoBehaviour.Instantiate(objectPrefab, position, Quaternion.LookRotation(_direction), plane.transform);
            newObject.AddComponent<BoxCollider>();
            newObject.AddComponent<WallObject>();
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
        newObjectLast.AddComponent<WallObject>();
        newObjectLast.transform.localScale = new Vector3(initialObjectScale.x, initialObjectScale.y, remainingLength);
        newObjectLast.tag = newObjectLast.name = "Wall";

        // Calculate the cost
        float cost = objectPrefab.GetComponent<BuildableObject>().GetCost();
        float totalCost = cost * numTiles;
        if (remainingLength != 0)
        {
            totalCost += cost;
        }

        gameVariableConnector.economyManagerScript.SubtractFromBalance(totalCost);
    }
}
