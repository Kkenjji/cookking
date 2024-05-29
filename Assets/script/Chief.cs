using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private float RotationSpeed = 10f;
    [SerializeField] private ChiefMovement chiefMovement;
    private void Update()
    {
        Vector2 InputVector = chiefMovement.Moving();
        Vector3 Position3D = new Vector3(InputVector.x, 0f, InputVector.y);
        float PlayerSize = 0.5f;
        float PlayerHeight = 2f;
        float Distance = Time.deltaTime * MoveSpeed;
        bool ObjectInfront = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerSize, Position3D, Distance);

        
        if (ObjectInfront)// when got object infront
        {
            Vector3 MoveLeft = new Vector3(Position3D.x, 0, 0);//try to move left
            ObjectInfront = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerSize, MoveLeft, Distance);
            if (!ObjectInfront)//if no object on left move left only 
            {
                Position3D = MoveLeft;
            }
            else// cannot move left so move right
            {
                Vector3 MoveRight = new Vector3(0, 0, Position3D.z);
                ObjectInfront = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerSize, MoveRight, Distance);
                if (!ObjectInfront)//move right
                {
                    Position3D = MoveRight;
                }
                else//do not move
                {
                }
                transform.forward = Vector3.Slerp(transform.forward, Position3D, Time.deltaTime * RotationSpeed);


            }
        }
        if (!ObjectInfront) //if no object can move
        {
            transform.position += Position3D * Time.deltaTime * MoveSpeed;
        }
    }
}
