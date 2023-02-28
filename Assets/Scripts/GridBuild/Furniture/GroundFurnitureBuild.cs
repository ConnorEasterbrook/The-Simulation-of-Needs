using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFurnitureBuild : GridBuildCore
{
    public void BuildObject()
    {
        Vector3 hitPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
        objectPosition = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
        previewObject.transform.position = objectPosition;

        if (!isInside)
        {
            previewObject.SetActive(false);
        }
        else
        {
            previewObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && previewObject.activeSelf && validRaycast && isInside)
        {
            InstantiateObject();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateObject(45);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateObject(-45);
        }
    }

    private void RotateObject(float _angle)
    {
        previewObject.transform.Rotate(0, _angle, 0);
    }

    /// <summary>
    /// Instantiates the object
    /// </summary>
    private void InstantiateObject()
    {
        Quaternion rotation = previewObject.transform.rotation;
        GameObject newObject = MonoBehaviour.Instantiate(objectPrefab, objectPosition, rotation, gameVariableConnector.furnitureParent.transform);
        
        gameVariableConnector.economyManagerScript.SubtractFromBalance(objectPrefab.GetComponent<BuildableObject>().GetCost());
    }
}
