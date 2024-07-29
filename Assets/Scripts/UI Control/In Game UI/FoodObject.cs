using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Customer;

public class FoodObject : MonoBehaviour
{
    public Food foodType;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
