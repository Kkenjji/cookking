using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plate : Base
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;


    public override void Interact(ChefController Chef)
    {
        if (!Chef.IsKitchenObject())//Chef not carrying any items
        {
            KitchenObject.SpawnFood(kitchenObjectScript, Chef);
            EventManager.TriggerKitchenObjectPickedUp(kitchenObjectScript);
        }

    }

}
   
    

