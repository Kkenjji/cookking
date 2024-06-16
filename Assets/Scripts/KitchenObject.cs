using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;

    private KitchenInterface kitchenInterface;

    public KitchenObjectScript GetKitchenObjectScript()
    {
        return kitchenObjectScript;
    }
    public void SetKitchenInterface(KitchenInterface kitchenInterface) {
        if (this.kitchenInterface != null) {
        this.kitchenInterface.ClearKitchenObject();//set new parent
        }
        this.kitchenInterface = kitchenInterface;

        kitchenInterface.SetKitchenObject(this);
        transform.parent = kitchenInterface.MovementPointTransform();
        transform.localPosition = Vector3.zero;
    }
    public KitchenInterface GetKitchenInterface() { 
        return kitchenInterface;
    }
}
