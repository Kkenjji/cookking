using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;
    [SerializeField] private Transform Countertop;
    [SerializeField] private EmptyCounter NextCounter;
    [SerializeField] private bool Test;
    private KitchenObject kitchenObject;


    private void Update()
    {
        if (Test && Input.GetKeyDown(KeyCode.K)) {
            if (kitchenObject != null) { 
                kitchenObject.SetEmptyCounter(NextCounter);//set new parent 
            }
        }
    }
    public void Interact(ChefController Chef)
    {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectScript.prefab, Countertop);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetEmptyCounter(this);
            
            //makes sure only one object is spawned each time 
        }
        else { //pass object to player
            
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
