using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateObject : KitchenObject
{
    public event Action<KitchenObjectScript> AddIngredient;
    public class AddIngredientEventArgs
    {
        public KitchenObjectScript KOS;
    }
    [SerializeField] private List<KitchenObjectScript> AllowedFood;
    private List<KitchenObjectScript> KOSList;

    private void Awake()
    {
        KOSList = new List<KitchenObjectScript>();
    }

    public bool IngredientOnPlate(KitchenObjectScript KOS)
    {
        if (!AllowedFood.Contains(KOS)) return false;//not in list
        if (KOSList.Contains(KOS))
        {
            return false;//ensure no duplicates
        }
        else
        {
            KOSList.Add(KOS);
            AddIngredient?.Invoke(
                KOS
            );
            return true;
        }
    }


    public void TriggerAllPickedUp()
    {
        foreach (var kOS in KOSList) { 
            EventManager.TriggerKitchenObjectPickedUp( kOS );
        }

    }

    public void TriggerAllRemoved()
    {
        foreach (var kOS in KOSList)
        {
            EventManager.TriggerKitchenObjectPlacedOrRemoved(kOS);
        }
    }


    public List<KitchenObjectScript> GetKOSList() {
    return KOSList;
    }
}
