using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private KitchenObjectScript kitchenObject;
    public KitchenObjectScript KitchenObjectScript=>kitchenObject;


    public void Initialize(KitchenObjectScript kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        iconImage.sprite = kitchenObject.presprite;

    }
}
