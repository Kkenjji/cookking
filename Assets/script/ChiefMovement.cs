using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ChiefMovement : MonoBehaviour
{
    private ChiefInput chiefInput;
    private void Awake()
    {
        chiefInput = new ChiefInput();
        chiefInput.Chief.Enable();
    }
    public Vector2 Moving()
    {
        Vector2 Input = chiefInput.Chief.Move.ReadValue<Vector2>();
        return Input;
    }
}
