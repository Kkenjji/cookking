using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
    }

    private IEnumerator test()
    {
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(FindObjectOfType<FoodTransferManager>().ShiftFood(Random.Range(0, 5)));
            yield return new WaitForSeconds(10f);
        }
    }
}
