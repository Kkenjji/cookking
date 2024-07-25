using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    private GridManager gridManager;
    private Dictionary<Vector2Int, bool> occupiedSeats = new Dictionary<Vector2Int, bool>();

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
        List<Node> nodeList = gridManager.GetSeats();
        foreach (Node node in nodeList)
        {
            Vector2Int coordinates = new Vector2Int(node.coords.x, node.coords.y);
            occupiedSeats.Add(coordinates, false);
            // Debug.Log(coordinates.x + "," + coordinates.y);
        }
    }

    public void Occupy(Vector2Int seat)
    {
        occupiedSeats[seat] = true;
        Debug.Log("Seat: (" + seat.x + " ," + seat.y + ") is occupied.");
    }

    public void FreeUp(Vector2Int seat)
    {
        occupiedSeats[seat] = false;
        Debug.Log("Seat: (" + seat.x + " ," + seat.y + ") is available.");
    }

    public bool isOccupied(Vector2Int seat)
    {
        return occupiedSeats[seat];
    }
}
