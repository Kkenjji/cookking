using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryBox : MonoBehaviour
{
    [SerializeField] private Transform itemsHolder;
    [SerializeField] private InventoryItem inventoryItemTemplate;

    [SerializeField] private GameObject plateIcon;
    private List<InventoryItem> InventoryItems;

    private void Awake()
    {
        EventManager.OnKitchenObjectPickedUp += AddInventoryItem;
        EventManager.OnKitchenObjectPlacedOrRemoved += CheckRemove;
    }
    private void OnDestroy()
    {
        EventManager.OnKitchenObjectPickedUp -= AddInventoryItem;
        EventManager.OnKitchenObjectPlacedOrRemoved -= CheckRemove;
    }
    private void Start()
    {
        plateIcon.SetActive(false);
    }
    private void AddInventoryItem(KitchenObjectScript script)
    {
        if(InventoryItems==null)InventoryItems = new List<InventoryItem>();
        if (script.Name.Equals("Plate"))
        {
            plateIcon.SetActive (true);
            return;
        }
        InventoryItem inventoryItem = Instantiate(inventoryItemTemplate, itemsHolder);
        inventoryItem.Initialize(script);
        InventoryItems.Add(inventoryItem);
    }



    private void CheckRemove(KitchenObjectScript script)
    {
        if (script.Name.Equals("Plate"))
        {
            plateIcon.SetActive(false);
            return;
        }

        InventoryItem itemToRemove = InventoryItems.FirstOrDefault(i=>i.KitchenObjectScript == script);
        if (itemToRemove != null)
        {
            InventoryItems.Remove(itemToRemove);
            Destroy(itemToRemove.gameObject);
        }
    }

}
