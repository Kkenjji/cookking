using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Base
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;


    public override void Interact(ChefController Chef)
    {
       
        Transform kitchenObjectTransform = Instantiate(kitchenObjectScript.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenInterface(Chef);

        }

    }
    

