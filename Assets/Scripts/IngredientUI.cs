using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientUI : MonoBehaviour
{
    [SerializeField] private PlateObject plateObject;
    [SerializeField] private Transform FoodIcon;

    private void Awake()
    {
        FoodIcon.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateObject.AddIngredient += PlateObject_AddIngredient;
    }

    private void PlateObject_AddIngredient(KitchenObjectScript kitchenObjectScript)
    {
        ChangeUI();
    }

    private void ChangeUI() {
        foreach (Transform child in transform) {
            if (child == FoodIcon) continue;
            Destroy(child.gameObject); 
        }

        foreach (KitchenObjectScript KOS in plateObject.GetKOSList())
        {
            Transform Icon = Instantiate(FoodIcon, transform);
            Icon.gameObject.SetActive(true);
            Icon.GetComponent<ChooseUI>().SetIngredient(KOS);
        }
    }
}
