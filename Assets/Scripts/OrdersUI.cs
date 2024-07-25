using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersUI : MonoBehaviour
{
    [SerializeField] private Transform Template;
    [SerializeField] private Transform Recipe;

    private void Awake()
    {
        Recipe.gameObject.SetActive(false);
    }

    private void Start()
    {
        OrderSystem.Instance.OrderSpawn += Instance_OrderSpawn;
        OrderSystem.Instance.OrderFinish += Instance_OrderFinish;
        UpdateVisual();
    }

    private void Instance_OrderFinish(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void Instance_OrderSpawn(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform i in Template) {
            if (i == Recipe) continue;
            Destroy(i.gameObject);
        }

        foreach (RecipeList recipeList in OrderSystem.Instance.GetRecipeList()) {
            Transform recipe = Instantiate(Recipe, Template);
            recipe.gameObject.SetActive(true);
            recipe.GetComponent<OrderPicUI>().SetRecipe(recipeList);
        }
    }
}

