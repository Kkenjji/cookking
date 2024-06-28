using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class QueueManager : MonoBehaviour
{
    public Transform[] queuePositions;
    public Queue<GameObject> queue = new Queue<GameObject>();
    public int capacity = 4;
    
    public void AddCustomer(GameObject newCustomer)
    {
        if (queue.Count < capacity)
        {
            Instantiate(newCustomer, queuePositions[queue.Count]);
            queue.Enqueue(newCustomer);
        }
    }

    public void RemoveCustomer(GameObject customer)
    {
        List<GameObject> temp = new List<GameObject>(queue);
        temp.Remove(customer);
        queue.Clear();
        foreach (GameObject tempCustomer in temp)
        {
            queue.Enqueue(tempCustomer);
        }
        UpdateQueue();
    }

    private void UpdateQueue()
    {
        int index = 0;
        foreach (GameObject customer in queue)
        {
            customer.transform.position = queuePositions[index].position;
            index++;
        }
    }
}
