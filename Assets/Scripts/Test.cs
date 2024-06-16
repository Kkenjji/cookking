using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Set Gizmo color
        Gizmos.color = Color.red;

        // Draw a line to represent the forward direction
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);

        // Optionally, draw an arrowhead or other indicators
        Gizmos.DrawSphere(transform.position + transform.forward * 2, 0.1f);
    }
}
