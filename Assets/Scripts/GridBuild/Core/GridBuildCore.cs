using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildCore
{
    public static GameObject objectPrefab;
    public static GameObject plane;
    public static Plane gridPlane;
    public static float tileSize = 1f;

    public static GameObject previewObject;
    public static Vector3 startPoint;
    public static Vector3 endPoint;
    public static Vector3 objectPosition;
    public static Vector3 initialObjectScale;

    public static Vector3 _direction;
    public static float _length;

    public static bool validRaycast = false;

    public static GameVariableConnector gameVariableConnector;
    public static EconomyManager economyManagerScript;

    public void PrepVariables(GameVariableConnector _gameVariableConnector, GameObject _prefab, GameObject _preview, GameObject _plane, Plane _gridPlane, float _tileSize)
    {
        gameVariableConnector = _gameVariableConnector;
        economyManagerScript = gameVariableConnector.economyManagerScript;

        objectPrefab = _prefab;
        previewObject = _preview;
        plane = _plane;
        gridPlane = _gridPlane;
        tileSize = _tileSize;
        initialObjectScale = objectPrefab.transform.localScale;
    }

    public void ChangeObject(GameObject _prefab, GameObject _preview)
    {
        previewObject.SetActive(false);
        objectPrefab = _prefab;
        previewObject = _preview;
        previewObject.SetActive(true);
        initialObjectScale = objectPrefab.transform.localScale;
    }

    public void UpdateValidity(bool _validRaycast)
    {
        validRaycast = _validRaycast;
    }

    /// <summary>
    /// Gets the mouse position on the grid plane
    /// </summary>
    public Vector3 GetMouseWorldPositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (gridPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Snaps the given point to the grid
    /// </summary>
    public Vector3 SnapToGrid(Vector3 hitPoint, bool wall = false)
    {
        float x = Mathf.Round(hitPoint.x / tileSize) * tileSize;
        float y = 0;
        float z = Mathf.Round(hitPoint.z / tileSize) * tileSize;

        if (wall)
        {
            y = hitPoint.y + (initialObjectScale.y * 0.5f);
        }
        else
        {
            y = hitPoint.y;
        }

        return new Vector3(x, y, z);
    }
}
