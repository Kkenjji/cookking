using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : Base
{
    public override void Interact(ChefController chef) {
        if (chef.IsKitchenObject())
        {
            EventManager.TriggerKitchenObjectPlacedOrRemoved(chef.GetKitchenObject().GetKitchenObjectScript());
            chef.GetKitchenObject().DestroyFood();
        }
    }
}
