using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private float RotationSpeed = 10f;
    [SerializeField] private ChiefMovement chiefMovement;
    [SerializeField] private float PlayerSize = 0.5f;
    private void Update()
    {
        Vector2 InputVector = chiefMovement.Moving();
        Vector3 Position3D = new Vector3(InputVector.x, 0f, InputVector.y);
        bool ObjectInfront = Physics.Raycast(transform.position, Position3D, PlayerSize);

        if (!ObjectInfront)
        {
            transform.position += Position3D * Time.deltaTime * MoveSpeed;
        }
        transform.forward = Vector3.Slerp(transform.forward, Position3D, Time.deltaTime * RotationSpeed);

        
    }
}
