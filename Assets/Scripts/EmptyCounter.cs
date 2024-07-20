using System.Collections;
using System.Collections.Generic;
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
                        GetKitchenObject().DestroyFood();//destroy object
                    }
                }
                else
                { //player not holding plate but fooditem
                    if (GetKitchenObject().CheckPlate(out plateObject))
                    {
                        if (plateObject.IngredientOnPlate(Chef.GetKitchenObject().GetKitchenObjectScript()))
                        {
                            Chef.GetKitchenObject().DestroyFood();
                        }
                    }
                }
            }
            else
            {//player not carrying something
                GetKitchenObject().SetKitchenInterface(Chef);
            }
        }

    }

}
