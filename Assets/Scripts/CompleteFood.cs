using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CompleteFood : MonoBehaviour
{
    [Serializable]
    public struct Ingredient_Visual
    {
        public KitchenObjectScript KOS;
        public GameObject gameObject;
    }
    [SerializeField] private PlateObject plateObject;
    [SerializeField] private List<Ingredient_Visual> IngredientVisualList;
    private void Start()
    {
        plateObject.AddIngredient += PlateObject_AddIngredient;
        foreach (Ingredient_Visual ingredient_Visual in IngredientVisualList)
        {
            ingredient_Visual.gameObject.SetActive(false);
        }
    }

    private void PlateObject_AddIngredient(object sender, PlateObject.AddIngredientEventArgs e)
    {
        foreach (Ingredient_Visual ingredient_Visual in IngredientVisualList)
        {
            if (ingredient_Visual.KOS == e.KOS)
            {
                ingredient_Visual.gameObject.SetActive(true);
            }
        }
    }
}

