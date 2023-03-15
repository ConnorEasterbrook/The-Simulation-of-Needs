using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuilder : GridBuildCore
{
    private bool isOnWall = false;

    public void BuildObject()
    {
        // Raycast mouse to make sure it's on a wall and ignores the door tag
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.tag == "Wall")
            {
                isOnWall = true;

                Vector3 correctRotaion = hit.transform.eulerAngles - new Vector3(0, 90, 0);
                previewObject.transform.eulerAngles = correctRotaion;

                Vector3 hitObjectPosition = SnapToGrid(hit.point, true);
                objectPosition = new Vector3(hitObjectPosition.x, 0, hitObjectPosition.z);
                previewObject.transform.position = new Vector3(hitObjectPosition.x, 0, hitObjectPosition.z);
            }
            else
            {
                isOnWall = false;
            }
        }
        else
        {
            isOnWall = false;
        }

        // Vector3 hitPoint = SnapToGrid(GetMouseWorldPositionOnPlane());
        // objectPosition = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
        // previewObject.transform.position = objectPosition;

        if (!isOnWall)
        {
            previewObject.SetActive(false);
        }
        else
        {
            previewObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && previewObject.activeSelf && validRaycast && isOnWall)
        {
            InstantiateObject();
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
        GameObject checkObject = MonoBehaviour.Instantiate(objectPrefab, objectPosition, rotation, gameVariableConnector.furnitureParent.transform);

        // Delete wall objects that are now covered by the door
        BoxCollider collider = checkObject.GetComponent<BoxCollider>();
        
        Collider[] colliders = Physics.OverlapBox(checkObject.transform.position, collider.size / 2, checkObject.transform.rotation, LayerMask.GetMask("Wall"));
        foreach (Collider col in colliders)
        {
            MonoBehaviour.Destroy(col.gameObject);
        }

        GameObject newObject = MonoBehaviour.Instantiate(objectPrefab, objectPosition, rotation, gameVariableConnector.furnitureParent.transform);

        gameVariableConnector.economyManagerScript.SubtractFromBalance(objectPrefab.GetComponent<BuildableObject>().GetCost());
    }
}
