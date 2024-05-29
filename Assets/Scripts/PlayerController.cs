using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    GridManager gridManager;
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
                if(hit.transform.tag == "Tile")
                {
                    Vector2Int hitTile = hit.transform.GetComponent<Labeller>().coords;
                    Vector3Int targetDest = new Vector3Int(hitTile.x, 1, hitTile.y);
                    agent.destination = targetDest;
                }
            }
        }
    }
}
