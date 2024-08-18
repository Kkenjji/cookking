using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : Base {

    [SerializeField] private Recipe[] RecipeArray;

    public override void Interact(ChefController Chef)
    {
        if (!IsKitchenObject())//nothing on counter
        {
            if (Chef.IsKitchenObject())
            {//Chef carrying object
                KitchenObjectScript current = Chef.GetKitchenObject().GetKitchenObjectScript();
                if (InRecipe(current))
                {
                    EventManager.TriggerKitchenObjectPlacedOrRemoved(current);
                    Chef.GetKitchenObject().SetKitchenInterface(this);//make sure only items with recipie can be set
                }
            }
            else
            {//chef not carrying anything 
            }
        }
        else//something on counter
        {
            if (Chef.IsKitchenObject())
            {//player carrying something

            }
            else
            {//player not carrying something
                EventManager.TriggerKitchenObjectPickedUp(GetKitchenObject().GetKitchenObjectScript());
                GetKitchenObject().SetKitchenInterface(Chef);
            }
        }

    }
    public override void Cut(ChefController Chef)
    {
        if (IsKitchenObject() && InRecipe(GetKitchenObject().GetKitchenObjectScript()))
        {//got item on and item is in recipie
        KitchenObjectScript output = OutputToInput(GetKitchenObject().GetKitchenObjectScript());
        GetKitchenObject().DestroyFood();
        KitchenObject.SpawnFood(output, this);
        }
    }

    private KitchenObjectScript OutputToInput(KitchenObjectScript input)
    {
        foreach (Recipe recipe in RecipeArray)
        {
            if (recipe.Original == input)
            {
                return recipe.New;
            }
        }
        return null;
    }

    private bool InRecipe(KitchenObjectScript input) {
        foreach (Recipe recipe in RecipeArray)
        {
            if (recipe.Original == input)
            {
                return true;
            }
        }
        return false;
    }
}
