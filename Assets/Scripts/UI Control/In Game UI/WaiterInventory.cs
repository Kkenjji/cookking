using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Customer;

public class WaiterInventory : MonoBehaviour
{
    public Image foodImage;
    public Food? foodType;
    public bool hasItem;

    // Start is called before the first frame update
    void Start()
    {
        ClearInventory();
    }

    public void PickUpItem(Food foodType, Sprite foodSprite)
    {
        if (!hasItem)
        {
            this.foodType = foodType;
            hasItem = true;
            FindObjectOfType<FoodTransferManager>().Picked();
            UpdateUI(foodSprite);
        }
    }

    public void DiscardItem()
    {
        if (hasItem)
        {
            ClearInventory();
        }
    }

    private void ClearInventory()
    {
        foodType = null;
        hasItem = false;
        UpdateUI(null);
    }

    private void UpdateUI(Sprite foodSprite)
    {
        if (hasItem)
        {
            foodImage.sprite = foodSprite;
            foodImage.enabled = true;
        }
        else
        {
            foodImage.enabled = false;
        }
    }
}
