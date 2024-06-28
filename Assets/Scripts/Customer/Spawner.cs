using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public QueueManager queueManager;
    [SerializeField] int counter;

    // Start is called before the first frame update
    void Start()
    {
        queueManager = GameObject.FindObjectOfType<QueueManager>();
        StartCoroutine(FirstSpawn());
    }

    private IEnumerator FirstSpawn()
    {
        yield return new WaitForSeconds(1);
        SpawnCustomer();
        StartCoroutine(SpawnRegularly());
    }

    private IEnumerator SpawnRegularly()
    {
        while (counter > 0)
        {
            if (queueManager.queue.Count < queueManager.capacity)
            {
                float spawnInterval = Random.Range(4f, 7f);
                yield return new WaitForSeconds(spawnInterval);
                SpawnCustomer();
            }
        }
    }

    private void SpawnCustomer()
    {
        int prefabIndex = Random.Range(0, customerPrefabs.Length);
        GameObject newCustomer = customerPrefabs[prefabIndex];
        queueManager.AddCustomer(newCustomer);
    }
}
