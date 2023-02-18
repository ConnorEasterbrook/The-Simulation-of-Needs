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

    public void PrepVariables(GameObject _prefab, GameObject _plane, Plane _gridPlane, float _tileSize)
    {
        objectPrefab = _prefab;
        previewObject = _prefab;
        plane = _plane;
        gridPlane = _gridPlane;
        tileSize = _tileSize;
        initialObjectScale = objectPrefab.transform.localScale;
    }

    public void ChangeObject(GameObject _prefab)
    {
        objectPrefab = _prefab;
        previewObject = _prefab;
        initialObjectScale = objectPrefab.transform.localScale;
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
    public Vector3 SnapToGrid(Vector3 hitPoint)
    {
        float x = Mathf.Round(hitPoint.x / tileSize) * tileSize;
        float y = hitPoint.y + (initialObjectScale.y * 0.5f);
        float z = Mathf.Round(hitPoint.z / tileSize) * tileSize;

        return new Vector3(x, y, z);
    }
}
