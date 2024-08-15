using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // StartCoroutine(SpawnFood(10));
    }

    private void SpawnFood(int num)
    {
        while (num > 0)
        {
            
            //FindObjectOfType<FoodTransferManager>().ShiftFood(Random.Range(0, 4));
        }
    }
}
