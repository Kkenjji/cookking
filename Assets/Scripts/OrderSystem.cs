using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class OrderSystem : MonoBehaviour
{
    private bool Subscribed = false;
    public event Action OrderSpawn;
    public event Action OrderFinish;
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

    /* private void ..Start()
     {
        *//* {
             if (Customer.Instance != null)
             {
                 Customer.Instance.BurgerOrder += Instance_BurgerOrder;
                 Customer.Instance.SandwichOrder += Instance_SandwichOrder;
                 Customer.Instance.SaladOrder += Instance_SaladOrder;
                 Customer.Instance.ChickenSetOrder += Instance_ChickenSetOrder;
                 Customer.Instance.LambSetOrder += Instance_LambSetOrder;
                 Debug.Log("Subscribed to Customer events");
             }
             else
             {
                 Debug.LogError("Customer instance is null");
             }
         }*//*
     }

     private void Instance_LambSetOrder(object sender, EventArgs e)
     {
         recipeList.Add(fullRecipeList.recipeLists[4]);
         Customer.Instance.LambSetOrder -= Instance_LambSetOrder;
         OrderSpawn?.Invoke(this, EventArgs.Empty);

         Debug.Log("Lamb");
     }

     private void Instance_ChickenSetOrder(object sender, EventArgs e)
     {
         recipeList.Add(fullRecipeList.recipeLists[1]);
         Customer.Instance.ChickenSetOrder -= Instance_ChickenSetOrder;
         OrderSpawn?.Invoke(this, EventArgs.Empty);

         Debug.Log("Chicken");
     }

     private void Instance_SaladOrder(object sender, EventArgs e)
     {
         recipeList.Add(fullRecipeList.recipeLists[2]);
         Customer.Instance.SaladOrder -= Instance_SaladOrder;
         OrderSpawn?.Invoke(this, EventArgs.Empty);

         Debug.Log("salad");
     }

     private void Instance_SandwichOrder(object sender, EventArgs e)
     {
         recipeList.Add(fullRecipeList.recipeLists[3]);
         Customer.Instance.SandwichOrder -= Instance_SandwichOrder;
         OrderSpawn?.Invoke(this, EventArgs.Empty);

         Debug.Log("Sandwich");
     }

     private void Instance_BurgerOrder(object sender, EventArgs e)
     {
         recipeList.Add(fullRecipeList.recipeLists[0]);
         Customer.Instance.BurgerOrder -= Instance_BurgerOrder;
         OrderSpawn?.Invoke(this, EventArgs.Empty);
         Debug.Log("Burger");
     }*/

    /*private void .Update()
    {
        *//*SpawnTimer -= Time.deltaTime;
        if (SpawnTimer <= 0) {
            SpawnTimer = SpawnTimerMax;
            if (recipeList.Count < MaxRecipe)
            {
                RecipeList recipelist = fullRecipeList.recipeLists[UnityEngine.Random.Range(0, fullRecipeList.recipeLists.Count)];
                recipeList.Add(recipelist);

                OrderSpawn?.Invoke(this, EventArgs.Empty);
            }
        }*//*
        if (!Subscribed)
        {

            Customer.Instance.BurgerOrder += Instance_BurgerOrder;
            Customer.Instance.SandwichOrder += Instance_SandwichOrder;
            Customer.Instance.SaladOrder += Instance_SaladOrder;
            Customer.Instance.ChickenSetOrder += Instance_ChickenSetOrder;
            Customer.Instance.LambSetOrder += Instance_LambSetOrder;
            Subscribed = true;
          
        }
        if (Customer.Instance == null) {
        Subscribed = false;
        } 
    }*/

    private void OnEnable()
    {
        EventManager.BurgerOrder += Instance_BurgerOrder;
        EventManager.SandwichOrder += Instance_SandwichOrder;
        EventManager.SaladOrder += Instance_SaladOrder;
        EventManager.ChickenSetOrder += Instance_ChickenSetOrder;
        EventManager.LambSetOrder += Instance_LambSetOrder;
    }

    private void Instance_LambSetOrder()
    {
        recipeList.Add(fullRecipeList.recipeLists[4]);
        Customer.Instance.LambSetOrder -= Instance_LambSetOrder;
        OrderSpawn?.Invoke();

        Debug.Log("Lamb");
    }

    private void Instance_ChickenSetOrder()
    {
        recipeList.Add(fullRecipeList.recipeLists[1]);
        Customer.Instance.ChickenSetOrder -= Instance_ChickenSetOrder;
        OrderSpawn?.Invoke();

        Debug.Log("Chicken");
    }

    private void Instance_SaladOrder()
    {
        recipeList.Add(fullRecipeList.recipeLists[2]);
        Customer.Instance.SaladOrder -= Instance_SaladOrder;
        OrderSpawn?.Invoke();

        Debug.Log("salad");
    }

    private void Instance_SandwichOrder()
    {
        recipeList.Add(fullRecipeList.recipeLists[3]);
        Customer.Instance.SandwichOrder -= Instance_SandwichOrder;
        OrderSpawn?.Invoke();

        Debug.Log("Sandwich");
    }

    private void Instance_BurgerOrder()
    {
        recipeList.Add(fullRecipeList.recipeLists[0]);
        Customer.Instance.BurgerOrder -= Instance_BurgerOrder;
        OrderSpawn?.Invoke();
        Debug.Log("Burger");
    }

    private void OnDisable()
    {
        EventManager.BurgerOrder -= Instance_BurgerOrder;
        EventManager.SaladOrder -= Instance_SaladOrder;
        EventManager.SandwichOrder -= Instance_SandwichOrder;
        EventManager.ChickenSetOrder -= Instance_ChickenSetOrder;
        EventManager.LambSetOrder -= Instance_LambSetOrder;
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
                        Food food = Food.WrongFood;
                        
                        //call function
                    }

                }
                if (Plate_Order) {
                    //correct order
                    Food food=recipeList[i].FoodType;
                    //whatever function
                    recipeList.RemoveAt(i);
                    OrderFinish?.Invoke();
                    return;

                }
            }
        }
    }

    public List<RecipeList> GetRecipeList() {
        return recipeList;
    }
}
