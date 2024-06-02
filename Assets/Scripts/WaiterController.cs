using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterController : MonoBehaviour
{
    GridManager gridManager;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);
            
            if(hasHit)
            {
                // if mouse click hits restaurant floor tile
                if(hit.transform.tag == "Tile")
                {
                    Vector2 hitTile = hit.transform.GetComponent<Labeller>().coords;
                    Vector3 targetDest = new Vector3(hitTile.x, 1, hitTile.y);
                    agent.destination = targetDest;
                }
            }
        }
    }
}
