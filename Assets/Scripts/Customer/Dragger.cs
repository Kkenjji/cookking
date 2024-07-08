using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    private GameObject currCustomer;
    private Camera camera2;
    private Vector3 initPosition;
    private int layerCount;
    private QueueManager queueManager;
    private SeatManager seatManager;

    // Start is called before the first frame update
    void Start()
    {
        layerCount = 5;
        camera2 = GameObject.Find("Camera 2").GetComponent<Camera>();
        queueManager = FindObjectOfType<QueueManager>();
        seatManager = FindObjectOfType<SeatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera2.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            // Debug.Log("Mouse button down. Has hit: " + hasHit);

            if (currCustomer == null)
            {
                if (hasHit && hit.collider.CompareTag("Customer"))
                {
                    // Debug.Log("Hit object: " + hit.collider.name);

                    currCustomer = hit.collider.gameObject;
                    initPosition = currCustomer.transform.position;
                    if (!currCustomer.GetComponent<Customer>().isSeated)
                    {
                        currCustomer.GetComponent<BoxCollider>().enabled = false;
                        Cursor.visible = false;
                    }
                    else
                    {
                        currCustomer = null;
                    }
                    
                    // Debug.Log("Picked up customer: " + currCustomer.name);
                }                
            }
            else
            {
                if (hasHit)
                {
                    GameObject hitObj = hit.collider.gameObject;
                    Vector3 seatPos = new Vector3(hitObj.transform.position.x, initPosition.y, hitObj.transform.position.z);
                    Vector2Int seat = new Vector2Int((int)seatPos.x, (int)seatPos.z);
                    string hitObjTag = hitObj.tag;

                    // Debug.Log("Trying to place customer on: " + hitObj.name + " with tag: " + hitObjTag);
                    
                    switch (hitObjTag)
                    {
                        case "Table Left":
                            seatPos.z += 1;
                            seat.y += 1;
                            SeatOrReturnToInitial(seat, seatPos);
                            break;
                        case "Table Right":
                            seatPos.x -= 1;
                            seatPos.z += 1;
                            seat.x -= 1;
                            seat.y += 1;
                            SeatOrReturnToInitial(seat, seatPos);
                            break;
                        case "Chair":
                            SeatOrReturnToInitial(seat, seatPos);
                            break;
                        default:
                            currCustomer.transform.position = initPosition;
                            // Debug.Log("Placed customer back to initial position");
                            break;
                    }
                }
                else
                {
                    currCustomer.transform.position = initPosition;
                    // Debug.Log("No valid position found, returned customer to initial position");
                }
                
                currCustomer.GetComponent<BoxCollider>().enabled = true;
                currCustomer = null;
                Cursor.visible = true;
            }               
        }

        if (currCustomer != null && !currCustomer.GetComponent<Customer>().isSeated)
        {
            Plane plane = new Plane(Vector3.up, initPosition.y);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 position = ray.GetPoint(distance);
                currCustomer.transform.position = new Vector3(position.x, initPosition.y + layerCount, position.z);
            }
        }
    }

    private void SeatOrReturnToInitial(Vector2Int seat, Vector3 seatPos)
    {
        if (!seatManager.isOccupied(seat))
        {
            currCustomer.transform.position = seatPos;
            queueManager.SeatCustomer(currCustomer);
            seatManager.Occupy(seat);
            // Debug.Log("Placed customer at Table Left");
        }
        else
        {
            currCustomer.transform.position = initPosition;
            // Debug.Log("Placed customer back to initial position");
        }
    }
}
