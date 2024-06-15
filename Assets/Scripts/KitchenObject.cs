using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;

    private EmptyCounter emptyCounter;

    public KitchenObjectScript GetKitchenObjectScript()
    {
        return kitchenObjectScript;
    }
    public void SetEmptyCounter(EmptyCounter emptyCounter) {
        if (this.emptyCounter != null) {
        this.emptyCounter.ClearKitchenObject();//set new parent
        }
        this.emptyCounter = emptyCounter;

        emptyCounter.SetKitchenObject(this);
        transform.parent = emptyCounter.MovementPointTransform();
        transform.localPosition = Vector3.zero;
    }
    public EmptyCounter GetEmptyCounter() { 
        return emptyCounter;
    }
}
