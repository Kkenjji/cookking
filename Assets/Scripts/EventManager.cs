using System;
using UnityEngine;

public static class EventManager 
{
    

    public static event Action BurgerOrder;
    public static event Action SandwichOrder;
    public static event Action SaladOrder;
    public static event Action ChickenSetOrder;
    public static event Action LambSetOrder;


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

    public static void TriggerLambSetOrder() {

        LambSetOrder?.Invoke();
    }

    public static void TriggerSaladOrder() {
    
        SaladOrder?.Invoke();
    }

}
