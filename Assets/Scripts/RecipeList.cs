using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class RecipeList : ScriptableObject
{
    public List<KitchenObjectScript> KOSList;
    public string RecipeName;
    public Food FoodType;
}
