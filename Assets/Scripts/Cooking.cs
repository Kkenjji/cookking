using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu()]
public class Cooking : ScriptableObject
{
    public KitchenObjectScript Original;
    public KitchenObjectScript New;
    public float FryingTimer;
}
