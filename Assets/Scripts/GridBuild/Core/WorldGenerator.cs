using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private int tileSize = 1;
    private GameObject tilePrefab;
    private GameObject[,] gridArray;
    private bool[,] gridCheckArray;
    private int gridX;
    private int gridY;
    private List<Vector2> wallTiles;

    private static GridBuildManager instance;
    private int tilesGenerated = 0;

    public void Initialize(GridBuildManager _gridBuildManager, int _tileSize, GameObject _tilePrefab, int _gridX, int _gridY)
    {
        wallTiles = new List<Vector2>();

        instance = _gridBuildManager;
        tileSize = _tileSize;
        tilePrefab = _tilePrefab;
        gridX = _gridX;
        gridY = _gridY;

        instance.transform.position = new Vector3(instance.transform.position.x - (gridX * tileSize) / 2, instance.transform.position.y, instance.transform.position.z);
        tilePrefab.GetComponent<MeshRenderer>().enabled = false;

        gridArray = new GameObject[gridX, gridY];
        gridCheckArray = new bool[gridX, gridY];
        Vector2 startPos = instance.transform.position;

        int randomXPos = Random.Range(0, gridX);
        int randomYPos = Random.Range(0, gridY);
        StartCoroutine(GenerateWorld(randomXPos, randomYPos, tilePrefab, startPos));
    }

    public IEnumerator GenerateWorld(int x, int y, GameObject tile, Vector2 startPos)
    {
        if (x < 0 || x >= gridX || y < 0 || y >= gridY)
        {
            yield break;
        }

        tile.isStatic = true;


        if (gridArray[x, y] == null)
        {
            GameObject tileGO = new GameObject();
            tileGO.layer = 6;
            tileGO.AddComponent<MeshFilter>().mesh = tile.GetComponent<MeshFilter>().sharedMesh;

            MeshRenderer meshRenderer = tile.GetComponent<MeshRenderer>();
            meshRenderer.material.color = Color.white;
            tileGO.AddComponent<MeshRenderer>().material = meshRenderer.sharedMaterial;

            tileGO.transform.localScale = new Vector3(tileSize, .25f, tileSize);
            tileGO.transform.position = new Vector3(startPos.x + (x * tileSize), -(tilePrefab.transform.localScale.y / 2f), startPos.y + (y * tileSize));
            tileGO.transform.parent = instance.transform;
            tileGO.name = "Tile_" + x + "_" + y;
            tileGO.AddComponent<BoxCollider>();
            gridArray[x, y] = tileGO;

            tilesGenerated++;
            if (tilesGenerated >= gridX * gridY)
            {
                StartCoroutine(FloodFill(0, 0, Color.white, Color.red));
            }

            yield return new WaitForSeconds(.075f);
            StartCoroutine(GenerateWorld(x + 1, y, tile, startPos));
            StartCoroutine(GenerateWorld(x - 1, y, tile, startPos));
            StartCoroutine(GenerateWorld(x, y + 1, tile, startPos));
            StartCoroutine(GenerateWorld(x, y - 1, tile, startPos));
        }
    }

    public IEnumerator FloodFill(int x, int y, Color oldColour, Color newColour)
    {
        if (x < 0 || x >= gridX || y < 0 || y >= gridY)
        {
            yield break;
        }

        // Check if there's a wall gameobject in the way by checking collisions at the top of the tile
        if (Physics.CheckBox(gridArray[x, y].transform.position + new Vector3(0, 1, 0), new Vector3(tileSize / 2, 1, tileSize / 2), Quaternion.identity, 1 << 7))
        {
            if (!wallTiles.Contains(new Vector2(x, y)))
            {
                wallTiles.Add(new Vector2(x, y));
            }

            gridArray[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
            gridCheckArray[x, y] = true;

            yield break;
        }


        if (gridArray[x, y].GetComponent<MeshRenderer>().material.color == oldColour)
        {
            gridArray[x, y].GetComponent<MeshRenderer>().material.color = newColour;
            gridCheckArray[x, y] = true;

            if (x == gridX - 1 && y == gridY - 1)
            {
                // instance.GetWorldInfo(gridArray, gridCheckArray, wallTiles);
                bool foundUncheckedTile = false;

                for (int i = 0; i < gridX; i++)
                {
                    for (int j = 0; j < gridY; j++)
                    {
                        if (gridCheckArray[i, j] == false)
                        {
                            StartCoroutine(FinalChecks(i, j, Color.black, Color.yellow));
                            foundUncheckedTile = true;
                            break;
                        }
                    }

                    if (foundUncheckedTile)
                    {
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(.05f);
            StartCoroutine(FloodFill(x + 1, y, oldColour, newColour));
            StartCoroutine(FloodFill(x - 1, y, oldColour, newColour));
            StartCoroutine(FloodFill(x, y + 1, oldColour, newColour));
            StartCoroutine(FloodFill(x, y - 1, oldColour, newColour));

            // Get diagonal neighbours
            StartCoroutine(FloodFill(x + 1, y + 1, oldColour, newColour));
            StartCoroutine(FloodFill(x - 1, y + 1, oldColour, newColour));
            StartCoroutine(FloodFill(x + 1, y - 1, oldColour, newColour));
            StartCoroutine(FloodFill(x - 1, y - 1, oldColour, newColour));
        }
    }

    private int runningChecks = 0;

    public IEnumerator FinalChecks(int x, int y, Color wallColour, Color roomColour)
    {
        if (x < 0 || x >= gridX || y < 0 || y >= gridY)
        {
            yield break;
        }

        runningChecks++;

        if (gridCheckArray[x, y] == false)
        {
            // Check if there's a wall gameobject in the way by checking collisions at the top of the tile
            if (Physics.CheckBox(gridArray[x, y].transform.position + new Vector3(0, 1, 0), new Vector3(tileSize / 2, 1, tileSize / 2), Quaternion.identity, 1 << 7))
            {
                if (!wallTiles.Contains(new Vector2(x, y)))
                {
                    wallTiles.Add(new Vector2(x, y));
                }

                gridArray[x, y].GetComponent<MeshRenderer>().material.color = wallColour;
                gridCheckArray[x, y] = true;

                runningChecks--;
                yield break;
            }

            gridArray[x, y].GetComponent<MeshRenderer>().material.color = roomColour;
            gridCheckArray[x, y] = true;

            yield return new WaitForSeconds(.025f);
            StartCoroutine(FinalChecks(x + 1, y, wallColour, roomColour));
            StartCoroutine(FinalChecks(x - 1, y, wallColour, roomColour));
            StartCoroutine(FinalChecks(x, y + 1, wallColour, roomColour));
            StartCoroutine(FinalChecks(x, y - 1, wallColour, roomColour));

            if (runningChecks == 1)
            {
                if (!CheckForUncheckedTiles(wallColour, roomColour))
                {
                    instance.GetWorldInfo(gridArray, gridCheckArray, wallTiles);
                }
            }
        }

        runningChecks--;
    }

    private bool CheckForUncheckedTiles(Color wallColour, Color roomColour)
    {
        // Randomize room colour
        roomColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                if (gridCheckArray[i, j] == false)
                {
                    StartCoroutine(FinalChecks(i, j, wallColour, roomColour));
                    return true;
                }
            }
        }

        return false;
    }
}
