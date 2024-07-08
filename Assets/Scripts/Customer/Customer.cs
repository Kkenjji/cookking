using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        InQueue, // Idle // patience
        ReadingMenu, // ReadMenu // readTime
        WaitingForOrderPickup, // RaiseHand // patience
        WaitingForFood, // Idle // patience
        Eating, // Eating // eatTime
        WaitingForBillCheck, // RaiseHand // patience
        Leave // Destroy
    }
    public CustomerState currState;
    public Animator animator;
    public int customerType;

    private float readTime = 7f;
    private float eatTime = 7f;
    private float patience = 20f;    
    
    public bool isActive = false;
    public bool isSeated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currState = CustomerState.InQueue;
        animator = GetComponent<Animator>();
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (currState != CustomerState.Leave)
        {
            switch (currState)
            {
                case CustomerState.InQueue:
                    yield return InQueue();
                    break;
                case CustomerState.ReadingMenu:
                    yield return ReadMenu();
                    break;
                case CustomerState.WaitingForOrderPickup:
                    yield return WaitForOrderPickup();
                    break;
                case CustomerState.WaitingForFood:
                    yield return WaitForFood();
                    break;
                case CustomerState.Eating:
                    yield return Eating();
                    break;
                case CustomerState.WaitingForBillCheck:
                    yield return WaitForBillCheck();
                    break;
            }
        }

        Leave();
    }

    private string GetAnimation(CustomerState state)
    {
        switch (state)
        {
            case CustomerState.InQueue:
                return "Customer_" + customerType + "_Idle";
            case CustomerState.ReadingMenu:
                return "Customer_" + customerType + "_ReadMenu";
            case CustomerState.WaitingForOrderPickup:
                return "Customer_" + customerType + "_RaiseHand";
            case CustomerState.WaitingForFood:
                return "Customer_" + customerType + "_Idle";
            case CustomerState.Eating:
                return "Customer_" + customerType + "_Eating";
            case CustomerState.WaitingForBillCheck:
                return "Customer_" + customerType + "_RaiseHand";
            case CustomerState.Leave:
                return "Customer_" + customerType + "_Wave";
            default:
                return "Customer_" + customerType + "_Idle";
        }
    }

    private IEnumerator InQueue()
    {
        // animator.Play(GetAnimation(CustomerState.InQueue)); // play animation
        SetActive(false);
        Debug.Log("isActive = " + isActive);
        yield return new WaitForSeconds(5f); // PatienceTimer(patience);
        // currState = CustomerState.ReadingMenu; // Leave
        if (currState != CustomerState.ReadingMenu)
        {
            currState = CustomerState.Leave;
        }
    }

    private IEnumerator ReadMenu()
    {
        // animator.Play(GetAnimation(CustomerState.ReadingMenu)); // play animation
        SetActive(true);
        Debug.Log("isActive = " + isActive);
        yield return new WaitForSeconds(5f);
        currState = CustomerState.WaitingForOrderPickup;
    }

    private IEnumerator WaitForOrderPickup()
    {
        // animator.Play(GetAnimation(CustomerState.WaitingForOrderPickup)); // play animation
        SetActive(true);
        Debug.Log(currState + "isActive = " + isActive);
        yield return new WaitForSeconds(5f);
        // currState = CustomerState.WaitingForFood; // Leave
        if (currState != CustomerState.WaitingForFood)
        {
            currState = CustomerState.Leave;
        }
    }
    
    private IEnumerator WaitForFood()
    {
        // animator.Play(GetAnimation(CustomerState.WaitingForFood)); // play animation
        SetActive(false);
        Debug.Log(currState + "isActive = " + isActive);
        yield return new WaitForSeconds(5f);
        // currState = CustomerState.Eating; // Leave
        if (currState != CustomerState.Eating)
        {
            currState = CustomerState.Leave;
        }
    }

    private IEnumerator Eating()
    {
        // animator.Play(GetAnimation(CustomerState.Eating)); // play animation
        SetActive(false);
        Debug.Log(currState + "isActive = " + isActive);
        yield return new WaitForSeconds(5f);
        currState = CustomerState.WaitingForBillCheck;
    }

    private IEnumerator WaitForBillCheck()
    {
        // animator.Play(GetAnimation(CustomerState.WaitingForBillCheck)); // play animation
        SetActive(true);
        Debug.Log(currState + "isActive = " + isActive);
        yield return new WaitForSeconds(5f);
        // currState = CustomerState.Leave; // Leave
        if (currState != CustomerState.Leave)
        {
            currState = CustomerState.Leave;
        }
    }

    private void Leave()
    {
        // animator.Play(GetAnimation(CustomerState.Leave)); // play animation
        SetActive(false);
        Debug.Log(currState + "isActive = " + isActive);
        Destroy(gameObject, 1f);
    }

    private void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void SetSeated()
    {
        isSeated = true;
    }

    private IEnumerator PatienceTimer(float duration, bool leaveOnTimeout = false)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (leaveOnTimeout)
        {
            currState = CustomerState.Leave; // Leave if patience runs out in interactable states
        }
        // For non-interactable states, transition to the next state is handled in their respective methods
    }

    public void Interact()
    {
        switch(currState)
        {
            case CustomerState.WaitingForOrderPickup:
                currState = CustomerState.WaitingForFood;
                SetActive(true);
                Debug.Log("Order picked up.");
                break;
            case CustomerState.WaitingForFood:
                currState = CustomerState.Eating;
                SetActive(false);
                Debug.Log("Food served.");
                break;
            case CustomerState.WaitingForBillCheck:
                currState = CustomerState.Leave;
                SetActive(false);
                Debug.Log("Bill checked.");
                break;
        }
    }
}
