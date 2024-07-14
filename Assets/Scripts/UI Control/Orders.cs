using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class Orders : MonoBehaviour
{
    public Queue<KeyValuePair<int, Food>> orderList = new Queue<KeyValuePair<int, Food>>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOrder(int id, Food foodType)
    {
        orderList.Enqueue(new KeyValuePair<int, Food>(id, foodType));
    }
}
