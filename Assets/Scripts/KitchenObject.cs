using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScript kitchenObjectScript;

    private KitchenInterface kitchenInterface;

    public KitchenObjectScript GetKitchenObjectScript()
    {
        return kitchenObjectScript;
    }
    public void SetKitchenInterface(KitchenInterface kitchenInterface)
    {
        if (this.kitchenInterface != null)
        {
            this.kitchenInterface.ClearKitchenObject();//set new parent
        }
        this.kitchenInterface = kitchenInterface;

        kitchenInterface.SetKitchenObject(this);
        transform.parent = kitchenInterface.MovementPointTransform();
        transform.localPosition = Vector3.zero;
    }
    public KitchenInterface GetKitchenInterface()
    {
        return kitchenInterface;
    }
    public void DestroyFood()
    {
        kitchenInterface.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnFood(KitchenObjectScript kitchenObjectScript, KitchenInterface kitchenInterface)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectScript.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenInterface(kitchenInterface);
        return kitchenObject;
    }

     public bool CheckPlate(out PlateObject plateObject)
    {
        if (this is PlateObject)
        {
            plateObject = this as PlateObject;
            return true;
        }
        else
        {
            plateObject = null;
            return false;
        }

    }




}
