using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private bool isSeated = false;
    private float timeMenu;
    private float timeEat;
    private float timeWave;
    [SerializeField] private int pax;
    // Start is called before the first frame update
    void Start()
    {
        timeMenu = Random.Range(7, 9);
        timeEat = Random.Range(11, 13);
        timeWave = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSeated)
        {
            Menu();
        }
    }

    void Menu()
    {
        // reading menu for 8 seconds
    }
}
