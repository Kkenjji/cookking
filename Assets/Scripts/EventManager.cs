using System;
using UnityEngine;

public static class EventManager 
{
    

    public static event Action BurgerOrder;
    public static event Action SandwichOrder;
    public static event Action SaladOrder;
    public static event Action ChickenSetOrder;
    public static event Action SteakOrder;




    public static event Action<KitchenObjectScript> OnKitchenObjectPickedUp;
    public static event Action<KitchenObjectScript> OnKitchenObjectPlacedOrRemoved;


    public static void TriggerBurgerOrder()
    {
        BurgerOrder?.Invoke();
    }

    public static void TriggerSandwichOrder() {

        SandwichOrder?.Invoke();
    }

    public static void TriggerChickenSetOrder() {

        ChickenSetOrder?.Invoke();
    }

    public static void TriggerSteakOrder() {

        SteakOrder?.Invoke();
    }

    public static void TriggerSaladOrder() {
    
        SaladOrder?.Invoke();
    }


    public static void TriggerKitchenObjectPickedUp(KitchenObjectScript kitchenObject)
    {
        OnKitchenObjectPickedUp?.Invoke(kitchenObject); 
    }

    public static void TriggerKitchenObjectPlacedOrRemoved(KitchenObjectScript kitchenObject)
    {
        OnKitchenObjectPlacedOrRemoved?.Invoke(kitchenObject);
    }
}
