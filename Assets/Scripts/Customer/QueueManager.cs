using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class QueueManager : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public Transform[] queuePositions;
    private Queue<GameObject> queue = new Queue<GameObject>();
    private int capacity = 4;

    void Start()
    {
        AddCustomer();
        AddCustomer();
        AddCustomer();
    }
    
    public void AddCustomer()
    {
        if (queue.Count < capacity)
        {
            int prefabIndex = Random.Range(0, customerPrefabs.Length);
            GameObject newCustomer = customerPrefabs[prefabIndex];
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

    public void UpdateQueue()
    {
        int index = 0;
        foreach (GameObject customer in queue)
        {
            customer.transform.position = queuePositions[index].position;
            index++;
        }
    }
}
