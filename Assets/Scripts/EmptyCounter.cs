using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmptyCounter : Base
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;




    public override void Interact(ChefController Chef)
    {
        if (!IsKitchenObject())//nothing on counter
        {
            if (Chef.IsKitchenObject())
            {//Chef carrying object
                //chef is carrying plate
                if (Chef.GetKitchenObject().CheckPlate(out PlateObject plate))
                {
                    //remove all plate ingredients from box
                    plate.TriggerAllRemoved();
                }
                EventManager.TriggerKitchenObjectPlacedOrRemoved(Chef.GetKitchenObject().GetKitchenObjectScript());  
                Chef.GetKitchenObject().SetKitchenInterface(this);
            }
            else
            {//chef not carrying anything 
            }
        }
        else//something on counter
        {
            if (Chef.IsKitchenObject())
            {//player carrying something
                if (Chef.GetKitchenObject().CheckPlate(out PlateObject plateObject)) //player holding plate
                {


                    if (plateObject.IngredientOnPlate(GetKitchenObject().GetKitchenObjectScript()))
                    {// add to plate 
                        //remove all plate ingredients from box
                        plateObject.TriggerAllRemoved();    
                        EventManager.TriggerKitchenObjectPlacedOrRemoved(plateObject.GetKitchenObjectScript());
                        GetKitchenObject().DestroyFood();//destroy object
                    }
                }
                else
                { //player not holding plate but fooditem
                    if (GetKitchenObject().CheckPlate(out plateObject))
                    {
                        if (plateObject.IngredientOnPlate(Chef.GetKitchenObject().GetKitchenObjectScript()))
                        {

                            EventManager.TriggerKitchenObjectPlacedOrRemoved(Chef.GetKitchenObject().GetKitchenObjectScript());
                            Chef.GetKitchenObject().DestroyFood();
                        }
                    }
                }
            }
            else
            {//player not carrying something

                //add all plate ingredients to box
                if (GetKitchenObject().CheckPlate(out PlateObject plate))
                {
                    plate.TriggerAllPickedUp();
                }
                EventManager.TriggerKitchenObjectPickedUp(GetKitchenObject().GetKitchenObjectScript());
                GetKitchenObject().SetKitchenInterface(Chef);
            }
        }

    }

}
