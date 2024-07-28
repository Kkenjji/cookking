using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class OrderQueue : MonoBehaviour
{
    public SpriteRenderer[] foodImages;
    public GameObject orderPrefab;
    public List<GameObject> orders = new List<GameObject>();

    public void AddOrder(int tableId, Food foodType)
    {
        Sprite foodImage = foodImages[(int)foodType].sprite;
        GameObject newOrder = Instantiate(orderPrefab, transform);
        newOrder.GetComponent<Order>().Setup(tableId, foodImage);
        orders.Add(newOrder);
    }

    public void RemoveOrder(int tableId)
    {
        foreach (GameObject o in orders)
        {
            if (o.GetComponent<Order>().tableId == tableId)
            {
                orders.Remove(o);
                Destroy(o);
                break;
            }
        }
    }
}
