using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;
    [SerializeField] private Transform Countertop;

    public void Interact()
    {
      Transform kitchenObjectTransform = Instantiate(kitchenObjectScript.prefab, Countertop);
        kitchenObjectTransform.localPosition = Vector3.zero;
        
    }
}
