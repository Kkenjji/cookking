using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateObject : KitchenObject
{
    public event EventHandler<AddIngredientEventArgs> AddIngredient;
    public class AddIngredientEventArgs : EventArgs
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
            AddIngredient?.Invoke(this, new AddIngredientEventArgs
            {
                KOS = KOS
            });
            return true;
        }
    }

    public List<KitchenObjectScript> GetKOSList() {
    return KOSList;
    }
}
