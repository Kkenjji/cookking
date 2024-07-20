using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


public class OrderSystem : MonoBehaviour
{
    public event EventHandler OrderSpawn;
    public event EventHandler OrderFinish;
    public static OrderSystem Instance
    {
        get; private set;
    }
    [SerializeField] private FullRecipeList fullRecipeList;
    private List<RecipeList> recipeList;
    private float SpawnTimer;
    private float SpawnTimerMax= 4f;
    private int MaxRecipe = 4;


    private void Awake()
    {
        Instance = this;
        recipeList = new List<RecipeList>();
    }
    private void Update()
    {
        SpawnTimer -= Time.deltaTime;
        if (SpawnTimer <= 0) {
            SpawnTimer = SpawnTimerMax;
            if (recipeList.Count < MaxRecipe)
            {
                RecipeList recipelist = fullRecipeList.recipeLists[UnityEngine.Random.Range(0, fullRecipeList.recipeLists.Count)];
                recipeList.Add(recipelist);

                OrderSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public void CheckContents(PlateObject plate) {
        for (int i = 0; i < recipeList.Count; i++) {
        RecipeList recipelist = recipeList[i];
            if (recipelist.KOSList.Count == plate.GetKOSList().Count) {//check if same num of ingre
                bool Plate_Order = true;
                foreach (KitchenObjectScript OrderKOS in recipelist.KOSList) {//cycle thru ingre in order
                    bool IngredientSame= false;
                    foreach (KitchenObjectScript PlateKOS in plate.GetKOSList()) {//cycle thru ingre on plate
                        if (PlateKOS == OrderKOS) {
                        IngredientSame = true;
                            break;
                        }
                    }
                    if (!IngredientSame) {
                        Plate_Order= false;
                    }

                }
                if (Plate_Order) {
                    //correct order
                    recipeList.RemoveAt(i);
                    OrderFinish?.Invoke(this, EventArgs.Empty);
                    return;

                }
            }
        }

        
    }

    public List<RecipeList> GetRecipeList() {
    return recipeList;
    }
}
