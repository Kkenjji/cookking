using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseUI : MonoBehaviour
{
    [SerializeField] private Image image;


    public void SetIngredient(KitchenObjectScript KOS) {
        image.sprite = KOS.sprite;
    }
}
