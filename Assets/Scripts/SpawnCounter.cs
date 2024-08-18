using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCounter : Base
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;


    public override void Interact(ChefController Chef)
    {
        if (!Chef.IsKitchenObject())//Chef not carrying any items
        {
            KitchenObject.SpawnFood(kitchenObjectScript,Chef);
            EventManager.TriggerKitchenObjectPickedUp(kitchenObjectScript);
        }

        }

    }
    

