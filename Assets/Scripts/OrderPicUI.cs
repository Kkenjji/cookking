using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderPicUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RecipeName;
    [SerializeField] private Transform Icon;
    [SerializeField] private Transform Ingredient;


    private void Awake()
    {
        Ingredient.gameObject.SetActive(false);
    }


    public void SetRecipe(RecipeList recipeList) {
        RecipeName.text = recipeList.RecipeName;

        foreach (Transform i in Icon) {
            if (i == Ingredient) continue;
            Destroy(i.gameObject);
        }
        foreach (KitchenObjectScript KOS in recipeList.KOSList) {
        Transform icon = Instantiate(Ingredient,Icon);
            icon.gameObject.SetActive(true);
            icon.GetComponent<Image>().sprite=KOS.sprite;
        }
    }
}
