using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Vector2Int startCoords;
    public Vector2Int StartCoords { get { return startCoords; } }

    [SerializeField] Vector2Int targetCoords;
    public Vector2Int TargetCoords { get { return targetCoords; } }

    Node startNode;
    Node targetNode;
    Node currentNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    Vector2Int[] searchOrder = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down}; // (1, 0), (-1, 0), (0, 1), (0, -1)

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        while (!gridManager.hasUpdated)
        {

        }
        grid = gridManager.Grid;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoords);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();

        BFS(coordinates);
        return BuildPath();
    }

    private void BFS(Vector2Int coordinates)
    {
        startNode.walkable = true;
        targetNode.walkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning == true)
        {
            currentNode = frontier.Dequeue();
            currentNode.visited = true;
            FindNeighbours();
            if (currentNode.coords == targetCoords)
            {
                isRunning = false;
            }
        }
    }

    private void FindNeighbours()
    {
        List<Node> neighbours = new List<Node>();
        
        foreach (Vector2Int dir in searchOrder)
        {
            Vector2Int neighbourCoords = currentNode.coords + dir;

            if (grid.ContainsKey(neighbourCoords))
            {
                neighbours.Add(grid[neighbourCoords]);
            }
        }

        foreach (Node neighbour in neighbours)
        {
            if (!reached.ContainsKey(neighbour.coords) && neighbour.walkable)
            {
                neighbour.parent = currentNode;
                reached.Add(neighbour.coords, neighbour);
                frontier.Enqueue(neighbour);
            }
        }
    }

    private List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;
        
        path.Add(currentNode);
        currentNode.onPath = true;

        while (currentNode.parent != null)
        {
            currentNode = currentNode.parent;
            path.Add(currentNode);
            currentNode.onPath = true;
        }

        path.Reverse();
        return path;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
    public void SetNewDestination(Vector2Int startCoordinates, Vector2Int targetCoordinates)
    {
        startCoords = startCoordinates;
        targetCoords = targetCoordinates;
        startNode = grid[this.startCoords];
        targetNode = grid[this.targetCoords];
        GetNewPath();
    }
}
