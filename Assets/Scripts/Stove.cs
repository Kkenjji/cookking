using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Stove : Base
{
    private enum State
    {
        Raw,
        Frying,
        Done,
        Burnt,
    }

    private State state;
    private float FryTime;
    [SerializeField] private Cooking[] CookingArray;
    [SerializeField] private BurntCooking[] BurntCookingArray;
    private Cooking cookings;
    private float BurntTime;
    private BurntCooking burntCooking;


    private void Start()
    {
        state = State.Raw;
    }

    private void Update()
    {
        if (IsKitchenObject())
        {
            switch (state)
            {

                case State.Raw:
                    break;
                case State.Frying:
                    FryTime += Time.deltaTime;

                    if (FryTime > cookings.FryingTimer)
                    {
                        //fried meat
                        GetKitchenObject().DestroyFood();
                        KitchenObject.SpawnFood(cookings.New, this);

                        state = State.Done;
                        BurntTime = 0f;
                        burntCooking = GetBurnt(GetKitchenObject().GetKitchenObjectScript());

                    }
                    break;
                case State.Done:
                    BurntTime += Time.deltaTime;

                    if (BurntTime > burntCooking.BurningTimer)
                    {
                        //fried meat
                        GetKitchenObject().DestroyFood();
                        KitchenObject.SpawnFood(burntCooking.New, this);
                        state = State.Burnt;
                    }
                    break;
                case State.Burnt:
                    break;
            }

        }
    }
    public override void Interact(ChefController Chef)
    {
        if (!IsKitchenObject())//nothing on counter
        {
            if (Chef.IsKitchenObject())
            {//Chef carrying object
                if (InCooking(Chef.GetKitchenObject().GetKitchenObjectScript()))
                {
                    Chef.GetKitchenObject().SetKitchenInterface(this);//make sure only items with recipie can be set
                    cookings = GetCooking(GetKitchenObject().GetKitchenObjectScript());
                    state = State.Frying;
                    FryTime = 0f;
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
                if (Chef.GetKitchenObject().CheckPlate(out PlateObject plateObject))
                {
                    //player holding plate

                    if (plateObject.IngredientOnPlate(GetKitchenObject().GetKitchenObjectScript()))
                    {// add to plate 
                        GetKitchenObject().DestroyFood();//destroy object
                        state = State.Raw;
                    }
                }
            }
            else
            {//player not carrying something
                GetKitchenObject().SetKitchenInterface(Chef);
                state = State.Raw;
            }
        }
    }
    private KitchenObjectScript OutputToInput(KitchenObjectScript input)
    {
        foreach (Cooking cooking in CookingArray)
        {
            if (cooking.Original == input)
            {
                return cooking.New;
            }
        }
        return null;
    }

    private bool InCooking(KitchenObjectScript input)
    {
        foreach (Cooking cooking in CookingArray)
        {
            if (cooking.Original == input)
            {
                return true;
            }
        }
        return false;
    }

    private Cooking GetCooking(KitchenObjectScript input)
    {
        foreach (Cooking cooking in CookingArray)
        {
            if (cooking.Original == input)
            {
                return cooking;
            }
        }
        return null;
    }

    private BurntCooking GetBurnt(KitchenObjectScript input)
    {
        foreach (BurntCooking burncooking in BurntCookingArray)
        {
            if (burncooking.Original == input)
            {
                return burncooking;
            }
        }
        return null;
    }
}
