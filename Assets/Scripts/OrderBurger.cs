using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBurger : MonoBehaviour
{
    public event EventHandler OrderSpawn;
    public event EventHandler OrderFinish;
    public static OrderSystem Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
