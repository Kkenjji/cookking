using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;

public class ChefMovement : MonoBehaviour
{
    public event EventHandler OnInteract;           
    private ChefInput chefInput;
    private void Awake()
    {
        chefInput = new ChefInput();
        chefInput.Chef.Enable();
        chefInput.Chef.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnInteract != null)
        {
            OnInteract(this, EventArgs.Empty);
        }
    }
    public Vector2 Moving()
    {
        Vector2 Input = chefInput.Chef.Move.ReadValue<Vector2>();
        Input = Input.normalized;
        return Input;
    }
}
