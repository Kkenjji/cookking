using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player2 : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Ensure the NavMeshAgent is not null
                if (_agent != null)
                {
                    // Set the destination's Y to the current Y position of the agent to keep it from moving up or down
                    Vector3 targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    _agent.SetDestination(targetPosition);
                }
                else
                {
                    Debug.LogWarning("NavMeshAgent component is missing.");
                }
            }
        }
    }
}



