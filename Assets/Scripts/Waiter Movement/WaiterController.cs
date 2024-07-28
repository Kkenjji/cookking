using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Customer;

public class WaiterController : MonoBehaviour
{
    private bool isMoving;
    public float movementSpeed;
    public Animator animator;
    
    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;
    private Camera camera2;
    private SeatManager seatManager;
    private FoodTransferManager ftm;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        gridManager = FindObjectOfType<GridManager>();
        seatManager = FindObjectOfType<SeatManager>();
        pathFinder = GetComponent<Pathfinding>(); // 
        animator = GetComponent<Animator>();
        camera2 = GameObject.Find("Camera 2").GetComponent<Camera>();
        ftm = FindObjectOfType<FoodTransferManager>();
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
                        if (!isMoving)
                        {
                            customerComponent.Interact();
                        }
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
                        if (!isMoving)
                        {
                            customerComponent.Interact();
                        }
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
                        if (!isMoving)
                        {
                            customerComponent.Interact();
                        }
                    }
                }

                if (hit.transform.tag == "Customer")
                {
                    MoveFromChair(hit);
                    Customer customerComponent = hit.transform.GetComponent<Customer>();
                    if (!isMoving)
                    {
                        customerComponent.Interact();
                    }
                }

                if (hit.transform.tag == "Power Up")
                {
                    MoveToPowerUp(hit);
                }

                if (hit.transform.tag == "Receiving Top")
                {
                    MoveFromChair(hit);
                }

                if (hit.transform.tag == "Food")
                {
                    MoveFromChair(hit);
                    if (!isMoving)
                    {
                        if (ftm.hasFood && !FindObjectOfType<WaiterInventory>().hasItem)
                        {
                            GameObject food = hit.collider.gameObject;
                            ftm.Picked();
                            Debug.Log(food == null);
                            Debug.Log(food.GetComponent<FoodObject>() == null);
                            Food foodType = food.GetComponent<FoodObject>().foodType;
                            Sprite foodSprite = food.GetComponent<FoodObject>().sprite;
                            FindObjectOfType<WaiterInventory>().PickUpItem(foodType, foodSprite);
                            Destroy(food);
                        }
                    }
                }

                if (hit.transform.tag == "Receiving Bottom")
                {
                    MoveFromTableLeft(hit);
                    if (!isMoving)
                    {
                        if (ftm.hasFood && !FindObjectOfType<WaiterInventory>().hasItem)
                        {
                            GameObject food = hit.collider.gameObject;
                            ftm.Picked();
                            Food foodType = food.GetComponent<FoodObject>().foodType;
                            Sprite foodSprite = food.GetComponent<FoodObject>().sprite;
                            FindObjectOfType<WaiterInventory>().PickUpItem(foodType, foodSprite);
                            Destroy(food);
                        }
                    }
                }

                if (hit.transform.tag == "Bin")
                {
                    Debug.Log("Hit bin.");
                    MoveToBin(hit);
                    if (!isMoving)
                    {
                        FindObjectOfType<WaiterInventory>().DiscardItem();
                    }
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
        isMoving = true;
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
        isMoving = false;
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

    private void MoveToBin(RaycastHit hit)
    {
        if (!PauseMenu.gameIsPaused)
        {
            Vector2Int targetCoords = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.z);
            targetCoords.y -= 1;
            Vector2Int startCoords = new Vector2Int((int) transform.position.x / gridManager.UnityGridSize,
                                                    (int) transform.position.z / gridManager.UnityGridSize);
            pathFinder.SetNewDestination(startCoords, targetCoords);
            RecalculatePath(true);
        }
    }
}
