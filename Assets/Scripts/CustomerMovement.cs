using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    private GameObject customerObj;
    private Vector3 initCustomerPosition;
    private GameObject tableSeat;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (customerObj == null)
            {
                if (hit.collider.CompareTag("Customer")) // check if it hits customer collider
                {
                    customerObj = hit.collider.gameObject;
                    initCustomerPosition = new Vector3(
                        customerObj.transform.position.x, 
                        customerObj.transform.position.y, 
                        customerObj.transform.position.z);
                    Cursor.visible = false;
                }
            } else {
                if (hit.collider.CompareTag("2 Pax Table") || hit.collider.CompareTag("4 Pax Table"))
                {
                    tableSeat = hit.collider.gameObject;
                    Vector3 tablePosition = tableSeat.transform.position;
                    customerObj.transform.position = new Vector3(tablePosition.x, tablePosition.y + 1, tablePosition.z);
                } else {
                    customerObj.transform.position = initCustomerPosition;
                }
                
                // Vector3 position = new Vector3(
                //     Input.mousePosition.x, 
                //     Input.mousePosition.y, 
                //     Camera.main.WorldToScreenPoint(customerObj.transform.position).z);
                // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                // customerObj.transform.position = new Vector3(worldPosition.x, 2f, worldPosition.z);

                customerObj = null;
                Cursor.visible = true;
            }
        }

        if (customerObj != null)
        {
            Vector3 position = new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                Camera.main.WorldToScreenPoint(customerObj.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            customerObj.transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMouseFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMouseNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMouseFar = Camera.main.ScreenToWorldPoint(screenMouseFar);
        Vector3 worldMouseNear = Camera.main.ScreenToWorldPoint(screenMouseNear);

        RaycastHit hit;
        Physics.Raycast(worldMouseNear, worldMouseFar - worldMouseNear, out hit);

        return hit;
    }
}
