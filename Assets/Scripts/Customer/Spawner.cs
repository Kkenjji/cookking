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

    // Start is called before the first frame update
    void Start()
    {
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
        GameObject newCustomer = Instantiate(customerPrefabs[Random.Range(0, 1)]);
        newCustomer.transform.SetParent(layer3.transform);
        queueManager.AddCustomer(newCustomer);
        totalCustomers--;
    }
}
