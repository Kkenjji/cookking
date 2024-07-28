using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public Tilemap layer3;
    public GameObject[] customerPrefabs;
    public QueueManager queueManager;
    public float spawnTimeMin;
    public float spawnTimeMax;
    public int totalCustomers;
    private bool[] tableNumbers = new bool[20];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            tableNumbers[i] = true;
        }
        queueManager = GameObject.FindObjectOfType<QueueManager>();
        StartCoroutine(FirstSpawn());
    }

    private IEnumerator FirstSpawn()
    {
        yield return new WaitForSeconds(1f);
        SpawnCustomer();
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (totalCustomers > 0)
        {
            if (queueManager.queue.Count < queueManager.capacity)
            {
                float spawnInterval = Random.Range(spawnTimeMin, spawnTimeMax);
                yield return new WaitForSeconds(spawnInterval);
                SpawnCustomer();
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void SpawnCustomer()
    {
        GameObject newCustomer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Length)]);
        newCustomer.transform.SetParent(layer3.transform);
        queueManager.AddCustomer(newCustomer);
        totalCustomers--;
    }

    public int GetTableNumber()
    {
        int tempNumber = UnityEngine.Random.Range(0, tableNumbers.Length);
        while (!tableNumbers[tempNumber])
        {
            tempNumber = UnityEngine.Random.Range(0, tableNumbers.Length);
        }
        tableNumbers[tempNumber] = false;
        return tempNumber + 1;
    }

    public void SetAvailable(int tableId)
    {
        tableNumbers[tableId - 1] = true;
    }
}
