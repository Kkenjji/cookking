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
            else {//chef not carrying anything 
            }
        }
        else//something on counter
        {
            if (Chef.IsKitchenObject())
            {//player carrying something

            }
            else {//player not carrying something
                GetKitchenObject().SetKitchenInterface(Chef);
            }
        }
        
    }
    
}
