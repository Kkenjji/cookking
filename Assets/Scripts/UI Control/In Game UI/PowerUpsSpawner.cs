using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpsSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public Tilemap layer5;
    public float spawnIntervalMin;
    public float spawnIntervalMax;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        List<Vector2Int> walkables = FindObjectOfType<GridManager>().GetWalkable();
        foreach (Vector2Int entry in walkables)
        {
            // Debug.Log("x: " + entry.x + ", y: " + entry.y);
        }

        while (true)
        {
            float spawnInterval = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            GameObject randomPowerUp = Instantiate(powerUpPrefabs[UnityEngine.Random.Range(0, powerUpPrefabs.Length)]);
            randomPowerUp.transform.SetParent(layer5.transform);

            Vector2Int randomTile = walkables[UnityEngine.Random.Range(0, walkables.Count)];
            Vector3 spawnPos = new Vector3(randomTile.x + 0.5f, layer5.transform.position.y, randomTile.y + 0.5f);

            randomPowerUp.transform.position = spawnPos;
        }
    }
}
