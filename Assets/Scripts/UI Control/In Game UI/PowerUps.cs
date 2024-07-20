using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public string powerUpType;
    public int value = 50;
    public float multiplier = 1.8f;
    public float duration = 6f;
    private static bool isBoosted = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waiter"))
        {
            PickUp(other);
        }
    }

    void PickUp(Collider Waiter)
    {
        FindObjectOfType<AudioManager>().PlaySFX("PickUp");

        switch (powerUpType)
        {
            case "Heart":
                AddHeart();
                break;
            case "Coin":
                AddToProfits();
                break;
            case "Speed":
                StartCoroutine(Boost());
                break;
        }
    }

    private void AddHeart()
    {
        FindObjectOfType<Health>().AddHealth();
        Destroy(gameObject);
    }

    private void AddToProfits()
    {
        Profits profits = FindObjectOfType<Profits>();
        profits.AddProfits(value);
        
        Destroy(gameObject);
    }

    private IEnumerator Boost()
    {
        if (!isBoosted)
        {
            WaiterController controller = FindObjectOfType<WaiterController>();

            isBoosted = true;
            controller.movementSpeed *= multiplier;
        
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            
            yield return new WaitForSeconds(duration);
            
            controller.movementSpeed /= multiplier;
            isBoosted = false;
        }

        Destroy(gameObject);
    }
}
