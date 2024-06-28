using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] private int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    public Tilemap layer2;
    public Tilemap layer5;

    public bool hasUpdated = false;

    private void Awake()
    {
        CreateGrid();
        // UpdateWalkability();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].walkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.parent = null;
            entry.Value.visited = false;
            entry.Value.onPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();

        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = (float)(coordinates.x * unityGridSize + 0.5);
        position.z = (float)(coordinates.y * unityGridSize + 0.8);

        return position;
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                grid.Add(coords, new Node(coords, true));
            }
        }
    }

    // public void UpdateWalkability()
    // {
    //     foreach (Transform tile in layer2.transform)
    //     {
    //         Vector2Int coordinates = new Vector2Int((int) tile.position.x, (int) tile.position.z);
    //         BlockNode(coordinates);
    //     }

    //     foreach (Transform tile in layer5.transform)
    //     {
    //         Vector2Int coordinates = new Vector2Int((int) tile.position.x, (int) tile.position.z);
    //         BlockNode(coordinates);
    //     }

    //     hasUpdated = true;
    // }
}
