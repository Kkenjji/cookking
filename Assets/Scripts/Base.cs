using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, KitchenInterface
{

    [SerializeField] private Transform Countertop;
    private KitchenObject kitchenObject;

    public virtual void Interact(ChefController Chef) {
        
    }

    public Transform MovementPointTransform()
    {
        return Countertop;
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
