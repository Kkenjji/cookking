using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

public class ChefController : MonoBehaviour
{
    public static ChefController Instance {
        get;
        private set;
    }
    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private float RotationSpeed = 10f;
    [SerializeField] private ChefMovement chefMovement;
    private Vector3 LastSeen;
    private EmptyCounter InteractedCounter;
    public event EventHandler<SelectedCounterEventArgs> SelectedCounter;
    public class SelectedCounterEventArgs : EventArgs
    {
        public EmptyCounter InteractedCounter;
    }

    private void Awake()
    {
        if (Instance != null) {
            Debug.Log("Multiplayer");
        }
        Instance = this;
    }

    private void Start()
    {
        
        chefMovement.OnInteract += ChefMovement_OnInteract;
    }

    private void ChefMovement_OnInteract(object sender, System.EventArgs e)
    {
        if (InteractedCounter != null)
        {
            InteractedCounter.Interact();
        }
    }
    private void Update()
    {
        Movement();
        Interaction();

    }

    private void Interaction()
    {
        Vector2 InputVector = chefMovement.Moving();
        Vector3 Position3D = new Vector3(InputVector.x, 0f, InputVector.y);
        float InteractionDistance = 2f;
        if (Position3D != Vector3.zero)
        {
            LastSeen = Position3D;
        }
        if (Physics.Raycast(transform.position, LastSeen, out RaycastHit raycasthit, InteractionDistance))
        {
            if (raycasthit.transform.TryGetComponent(out EmptyCounter emptyCounter))
            {
                if (emptyCounter != InteractedCounter)
                {
                    SetInteractedCounter(emptyCounter);
                }

            }

            else
            {
                
                SetInteractedCounter( null);
            }
        }

        else
        {
            
            SetInteractedCounter(null);
        }

    }

    private void Movement()
    {
        Vector2 InputVector = chefMovement.Moving();
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


    private void SetInteractedCounter(EmptyCounter InteractedCounter)
    {
        this.InteractedCounter = InteractedCounter;

        SelectedCounter?.Invoke(this, new SelectedCounterEventArgs
        {
            InteractedCounter = InteractedCounter
        });
        

    }
}
