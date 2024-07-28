using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class FoodTransferManager : MonoBehaviour
{
    public Sprite[] foodImages;
    public GameObject foodPrefab;
    public bool hasFood;
    public Transform sendingCounter;
    public Transform receivingCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator ShiftFood(Food foodType)
    {
        if (!hasFood)
        {
            hasFood = true;
            Sprite foodImage = foodImages[(int)foodType];
            GameObject newFood = Instantiate(foodPrefab, sendingCounter);
            newFood.GetComponent<SpriteRenderer>().sprite = foodImage;
            newFood.GetComponent<FoodObject>().SetFoodObject(foodType, foodImage);
            yield return new WaitForSeconds(1f);
            newFood.transform.position = receivingCounter.position;
        }
    }

    public void Picked()
    {
        hasFood = false;
    }
}
