using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;

public class ChefMovement : MonoBehaviour
{
    public event EventHandler OnInteract;
    public event EventHandler OnCut;
    private ChefInput chefInput;
    private void Awake()
    {
        chefInput = new ChefInput();
        chefInput.Chef.Enable();
        chefInput.Chef.Interact.performed += Interact_performed;
        chefInput.Chef.Cut.performed += Cut_performed;
    }

    private void Cut_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCut?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this,EventArgs.Empty);
    }
    public Vector2 Moving()
    {
        Vector2 Input = chefInput.Chef.Move.ReadValue<Vector2>();
        Input = Input.normalized;
        return Input;
    }
}
