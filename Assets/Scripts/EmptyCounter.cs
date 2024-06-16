using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCounter : MonoBehaviour, KitchenInterface
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;
    [SerializeField] private Transform Countertop;
    private KitchenObject kitchenObject;


    
    public void Interact(ChefController Chef)
    {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectScript.prefab, Countertop);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenInterface(this);
            
            //makes sure only one object is spawned each time 
        }
        else { //pass object to player
            kitchenObject.SetKitchenInterface(Chef);
        }
        
    }
    public Transform MovementPointTransform() {
        return Countertop;
    }
    public void SetKitchenObject(KitchenObject kitchenObject) { 
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject() { 
        return kitchenObject;
    }
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool IsKitchenObject() { 
        return kitchenObject != null;
    }
}
