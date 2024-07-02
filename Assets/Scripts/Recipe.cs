using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public KitchenObjectScript Original;
    public KitchenObjectScript New;
}
