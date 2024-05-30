using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ChefMovement : MonoBehaviour
{
    private ChefInput chefInput;
    private void Awake()
    {
        chefInput = new ChefInput();
        chefInput.Chef.Enable();
    }
    public Vector2 Moving()
    {
        Vector2 Input = chefInput.Chef.Move.ReadValue<Vector2>();
        Input = Input.normalized;
        return Input;
    }
}
