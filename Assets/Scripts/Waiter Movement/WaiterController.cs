using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaiterController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    public Animator animator;
    
    [SerializeField] GameObject waiter;
    Transform wTransform;
    
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
        pathFinder = FindObjectOfType<Pathfinding>();
        camera2 = GameObject.Find("Camera 2").GetComponent<Camera>();
        wTransform = waiter.transform;
        animator = waiter.GetComponent<Animator>();
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
            Vector3 startPos = wTransform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coords);
            endPos.y = wTransform.position.y;
            float travelPercent = 0f;

            // Debug.Log($"Moving from {startPos} to {endPos}");
            Vector3 direction = endPos - startPos;

            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.z);
            animator.SetFloat("Speed", movementSpeed);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                wTransform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame(); 
            }
        }

        animator.SetFloat("Speed", 0);
        // Debug.Log("Idle");
    }
    
    private void MoveFromChair(RaycastHit hit)
    {
        Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
        targetCoords.x += 1;
        Vector2Int startCoords = new Vector2Int((int) wTransform.position.x / gridManager.UnityGridSize,
                                                (int) wTransform.position.z / gridManager.UnityGridSize);
        pathFinder.SetNewDestination(startCoords, targetCoords);
        RecalculatePath(true);
    }

    private void MoveFromTableLeft(RaycastHit hit)
    {
        Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
        targetCoords.x += 1;
        targetCoords.y += 1;
        Vector2Int startCoords = new Vector2Int((int) wTransform.position.x / gridManager.UnityGridSize,
                                                (int) wTransform.position.z / gridManager.UnityGridSize);
        pathFinder.SetNewDestination(startCoords, targetCoords);
        RecalculatePath(true);
    }

    private void MoveFromTableRight(RaycastHit hit)
    {
        Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
        targetCoords.y += 1;
        Vector2Int startCoords = new Vector2Int((int) wTransform.position.x / gridManager.UnityGridSize,
                                                (int) wTransform.position.z / gridManager.UnityGridSize);
        pathFinder.SetNewDestination(startCoords, targetCoords);
        RecalculatePath(true);
    }
}
