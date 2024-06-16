using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface KitchenInterface
{
    public Transform MovementPointTransform();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();


    public bool IsKitchenObject();

}
