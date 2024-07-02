using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int coords;
    public bool walkable;
    public bool visited;
    public bool onPath;
    public Node parent;   
    public Node(Vector2Int coords, bool walkable)
    {
        this.coords = coords;
        this.walkable = walkable;
    }
}
