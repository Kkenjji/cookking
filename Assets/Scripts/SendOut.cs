using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendOut : Base
{
    public override void Interact(ChefController Chef)
    {
        if (Chef.IsKitchenObject()) {
            if (Chef.GetKitchenObject().CheckPlate(out PlateObject plateObject)) {// make sure only take in plate
                //remove all ingredients from plate as well as the plate
                plateObject.TriggerAllRemoved();
                EventManager.TriggerKitchenObjectPlacedOrRemoved(plateObject.GetKitchenObjectScript()); 

                if (!FindObjectOfType<FoodTransferManager>().hasFood)
                {
                    OrderSystem.Instance.CheckContents(plateObject);
                    Chef.GetKitchenObject().DestroyFood();
                }
            }
        }
    }
}
