using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaiterController : MonoBehaviour
{
    public float movementSpeed;
    public Animator animator;
    
    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;
    private Camera camera2;
    private SeatManager seatManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        seatManager = FindObjectOfType<SeatManager>();
        pathFinder = GetComponent<Pathfinding>(); // 
        animator = GetComponent<Animator>();
        camera2 = GameObject.Find("Camera 2").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera2.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);
            
            if (hasHit && Cursor.visible == true)
            {
                if (hit.transform.tag == "Chair")
                {
                    MoveFromChair(hit);
                    Vector2Int chairPos = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y);
                    if (seatManager.isOccupied(chairPos))
                    {
                        Customer customerComponent = hit.transform.GetComponent<Customer>();
                        customerComponent.Interact();
                    }
                }
                
                if (hit.transform.tag == "Table Left")
                {
                    MoveFromTableLeft(hit);
                    Vector2Int chairPos = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
                    chairPos = chairPos + Vector2Int.up;
                    if (seatManager.isOccupied(chairPos))
                    {
                        Customer customerComponent = hit.transform.GetComponent<Customer>();
                        if (customerComponent == null)
                        {
                            Debug.LogWarning("Customer Component not detected on hit object");
                            return;
                        }
                        customerComponent.Interact();
                    }
                }
                
                if (hit.transform.tag == "Table Right")
                {
                    MoveFromTableRight(hit);
                    Vector2Int chairPos = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
                    chairPos = chairPos + Vector2Int.left + Vector2Int.up;
                    if (seatManager.isOccupied(chairPos))
                    {
                        Customer customerComponent = hit.transform.GetComponent<Customer>();
                        customerComponent.Interact();
                    }
                }

                if (hit.transform.tag == "Customer")
                {
                    MoveFromChair(hit);
                    Customer customerComponent = hit.transform.GetComponent<Customer>();
                    customerComponent.Interact();
                }

                if (hit.transform.tag == "Power Up")
                {
                    MoveToPowerUp(hit);
                }
            }
        }
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int newStartCoords = new Vector2Int();
        if (resetPath)
        {
            newStartCoords = pathFinder.StartCoords;
        }
        else
        {
            newStartCoords = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(newStartCoords);
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coords);
            endPos.y = transform.position.y;
            float travelPercent = 0f;

            // Debug.Log($"Moving from {startPos} to {endPos}");
            Vector3 direction = endPos - startPos;

            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.z);
            animator.SetFloat("Speed", movementSpeed);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame(); 
            }
        }
        animator.SetFloat("Speed", 0);
        // Debug.Log("Idle");
    }
    
    private void MoveFromChair(RaycastHit hit)
    {
        if (!PauseMenu.gameIsPaused)
        {
            Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
            targetCoords.x += 1;
            Vector2Int startCoords = new Vector2Int((int) transform.position.x / gridManager.UnityGridSize,
                                                    (int) transform.position.z / gridManager.UnityGridSize);
            pathFinder.SetNewDestination(startCoords, targetCoords);
            RecalculatePath(true);
        }
    }

    private void MoveFromTableLeft(RaycastHit hit)
    {
        if (!PauseMenu.gameIsPaused)
        {
            Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
            targetCoords.x += 1;
            targetCoords.y += 1;
            Vector2Int startCoords = new Vector2Int((int) transform.position.x / gridManager.UnityGridSize,
                                                    (int) transform.position.z / gridManager.UnityGridSize);
            pathFinder.SetNewDestination(startCoords, targetCoords);
            RecalculatePath(true);
        }
    }

    private void MoveFromTableRight(RaycastHit hit)
    {
        if (!PauseMenu.gameIsPaused)
        {
            Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
            targetCoords.y += 1;
            Vector2Int startCoords = new Vector2Int((int) transform.position.x / gridManager.UnityGridSize,
                                                    (int) transform.position.z / gridManager.UnityGridSize);
            pathFinder.SetNewDestination(startCoords, targetCoords);
            RecalculatePath(true);
        }
    }

    private void MoveToPowerUp(RaycastHit hit)
    {
        if (!PauseMenu.gameIsPaused)
        {
            Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
            Vector2Int startCoords = new Vector2Int((int) transform.position.x / gridManager.UnityGridSize,
                                                    (int) transform.position.z / gridManager.UnityGridSize);
            pathFinder.SetNewDestination(startCoords, targetCoords);
            RecalculatePath(true);
        }
    }
}
