using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class WaiterInventory : MonoBehaviour
{
    public Food currentFoodType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetFood(Food foodType)
    {
        currentFoodType = foodType;
    }
}
