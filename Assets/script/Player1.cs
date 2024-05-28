using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 8f;
    private void Update()
    {
        Vector2 Position2D = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            Position2D.y = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Position2D.y = -1;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Position2D.x = 1;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            Position2D.x = -1;
        }
        Position2D = Position2D.normalized;
        Vector3 Position3D = new Vector3(Position2D.x, 0f, Position2D.y);
        transform.position += Position3D*Time.deltaTime*MoveSpeed;
        transform.forward=Position3D;




    }
}
