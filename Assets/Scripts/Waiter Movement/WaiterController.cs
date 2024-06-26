using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaiterController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    
    [SerializeField] GameObject waiter;
    Transform wTransform;
    
    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;
    private Camera camera2;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();
        camera2 = GameObject.Find("Camera 2").GetComponent<Camera>();
        wTransform = waiter.transform;
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
                if(hit.transform.tag != "Blocked")
                {
                    Vector2Int targetCoords = hit.transform.GetComponent<Tile>().coords;
                    Vector2Int startCoords = new Vector2Int((int) wTransform.position.x / gridManager.UnityGridSize,
                                                            (int) wTransform.position.z / gridManager.UnityGridSize);
                    pathFinder.SetNewDestination(startCoords, targetCoords);
                    RecalculatePath(true);
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

            Debug.Log($"Moving from {startPos} to {endPos}");
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                wTransform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame(); 
            }
        }
    }
}
