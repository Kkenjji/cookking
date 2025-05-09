using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;
using Unity.VisualScripting;

public class ChefController : MonoBehaviour, KitchenInterface
{
    public static ChefController Instance {
        get;
        private set;
    }

    [SerializeField] private float MoveSpeed = 8f;
    [SerializeField] private ChefMovement chefMovement;
    [SerializeField] private Animator chefAnimator;
    [SerializeField] private Transform KitchenObjectHold;
    
    
    
    private Vector3 LastSeen;
    private Base InteractedCounter;
    public event Action<Base> SelectedCounter;
    private KitchenObject kitchenObject;

    // animation parameter constants
    private const string isMovingStr = "IsMoving";
    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";

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
        chefMovement.OnCut += ChefMovement_OnCut;
    }

    private void ChefMovement_OnCut()
    {
        if (InteractedCounter != null)
        {
            InteractedCounter.Cut(this);
        }
    }


    private void ChefMovement_OnInteract()
    {
        if (InteractedCounter != null)
        {
            InteractedCounter.Interact(this);
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
        float InteractionDistance = 1f;
        if (Position3D != Vector3.zero)
        {
            LastSeen = Position3D;
        }
        if (Physics.Raycast(transform.position, LastSeen, out RaycastHit raycasthit, InteractionDistance))
        {
            if (raycasthit.transform.TryGetComponent(out Base bAse))
            {
                if (bAse != InteractedCounter)
                {
                    SetInteractedCounter(bAse);
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
        Vector2 inputVector = chefMovement.Moving();
        Vector3 Position3D = new Vector3(inputVector.x, 0f, inputVector.y);
        float PlayerSize = 0.5f;
        float PlayerHeight = 2f;
        float Distance = Time.deltaTime * MoveSpeed;
        bool ObjectInfront = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerSize, Position3D, Distance);
        bool isMoving = inputVector.magnitude > 0;
        chefAnimator.SetBool(isMovingStr, isMoving); // set based on whether player is moving or not
        chefAnimator.SetFloat(horizontal, inputVector.x);
        chefAnimator.SetFloat(vertical, inputVector.y);

        if (isMoving)
        {
            chefAnimator.SetFloat(lastHorizontal, inputVector.x);
            chefAnimator.SetFloat(lastVertical, inputVector.y);
        }

        if (ObjectInfront)// when got object infront
        {
            Vector3 MoveLeft = new Vector3(Position3D.x, 0, 0);//try to move left
            ObjectInfront = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerSize, MoveLeft, Distance);
            if (!ObjectInfront)//if no object on left move left only 
            {
                Position3D = MoveLeft;
            }
            else // cannot move left so move right
            {
                Vector3 MoveRight = new Vector3(0, 0, Position3D.z);
                ObjectInfront = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerSize, MoveRight, Distance);
                if (!ObjectInfront) // move right
                {
                    Position3D = MoveRight;
                }
                else // do not move
                {
                    Position3D = Vector3.zero;
                }
           }
        }
        
        if (!ObjectInfront) // if no object can move
        {
            transform.position += Position3D * Time.deltaTime * MoveSpeed;
        }
    }


    private void SetInteractedCounter(Base InteractedCounter)
    {
        this.InteractedCounter = InteractedCounter;
        SelectedCounter?.Invoke(InteractedCounter);
    }

    public Transform MovementPointTransform()
    {
        return KitchenObjectHold;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool IsKitchenObject()
    {
        return kitchenObject != null;
    }
}
