using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu()]
public class BurntCooking : ScriptableObject
{
    public KitchenObjectScript Original;
    public KitchenObjectScript New;
    public float BurningTimer;
}
