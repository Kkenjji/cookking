using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class FoodTransferManager : MonoBehaviour
{
    public GameObject[] foods;
    public bool hasFood;
    public Transform sendingCounter;
    public Transform receivingCounter;

    public IEnumerator ShiftFood(Food foodType)
    {
        if (!hasFood)
        {
            hasFood = true;
            GameObject newFood = Instantiate(foods[(int)foodType], sendingCounter);
            yield return new WaitForSeconds(1f);
            newFood.transform.position = receivingCounter.position;
        }
    }

    public void Picked()
    {
        hasFood = false;
    }

    public void Penalty()
    {
        FindObjectOfType<Profits>().Penalty();
    }
}
