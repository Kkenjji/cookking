using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public QueueManager queueManager;
    [SerializeField] float spawnTimeMin;
    [SerializeField] float spawnTimeMax;
    [SerializeField] int totalCustomers;

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
        int prefabIndex = Random.Range(0, customerPrefabs.Length - 1);
        GameObject newCustomer = Instantiate(customerPrefabs[prefabIndex]);
        queueManager.AddCustomer(newCustomer);
        totalCustomers--;
    }
}
