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
        transform.position += Position3D*Time.deltaTime*MoveSpeed;
        transform.forward=Vector3.Slerp(transform.forward,Position3D,Time.deltaTime*RotationSpeed);





    }
}
