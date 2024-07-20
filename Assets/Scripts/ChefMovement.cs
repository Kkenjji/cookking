using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class ChefMovement : MonoBehaviour
{
    public event EventHandler OnInteract;
    public event EventHandler OnCut;
    public event EventHandler OnPause;
    private ChefInput chefInput;
    public static ChefMovement Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        chefInput = new ChefInput();
        chefInput.Chef.Enable();
        chefInput.Chef.Interact.performed += Interact_performed;
        chefInput.Chef.Cut.performed += Cut_performed;
        chefInput.Chef.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        chefInput.Chef.Interact.performed -= Interact_performed;
        chefInput.Chef.Cut.performed -= Cut_performed;
        chefInput.Chef.Pause.performed -= Pause_performed;
        chefInput.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
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
