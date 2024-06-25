using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class WaiterController : MonoBehaviour
{
    GridManager gridManager;
    Pathfinding pathFinder;
    private Camera camera2;
    [SerializeField] GameObject waiter;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;
    List<Node> path = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();
        camera2 = GameObject.Find("Camera 2").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = camera2.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);
            
            if(hasHit)
            {
                // if mouse click hits walkable tiles
                if(hit.transform.tag == "Tile")
                {
                    Vector2Int targetCoords = hit.transform.GetComponent<Tile>().coords;
                    Vector2Int startCoords = new Vector2Int((int) waiter.transform.position.x, 
                                                            (int) waiter.transform.position.y) / gridManager.UnityGridSize;

                    pathFinder.SetNewDestination(startCoords, targetCoords);
                    //waiter.transform.position = new Vector3(targetCoords.x, waiter.transform.position.y, targetCoords.y);
                    RecalculatePath(true);
                }
            }
        }
    }

    // private bool CheckWithin(Vector2 hitPosition)
    // {
    //     int gridX = gridManager.GridSize.x;
    //     int gridY = gridManager.GridSize.y;
    //     if (hitPosition.x < 0 || hitPosition.x > gridX || hitPosition.y < 0 || hitPosition.y > gridY)
    //     {
    //         return false;
    //     }
    //     return true;
    // }

    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCoords;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(waiter.transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 startPos = waiter.transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coords);
            float travelPercent = 0f;

            waiter.transform.LookAt(endPos);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                waiter.transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame(); 
            }
        }
    }
}
