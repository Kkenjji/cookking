using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class FoodObject : MonoBehaviour
{
    public Food foodType;
    public Sprite foodSprite;
    public void SetFoodObject(Food foodType, Sprite foodSprite)
    {
        this.foodType = foodType;
        this.foodSprite = foodSprite;
    }
}
