using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        InQueue, // Idle
        ReadingMenu, // Read Menu
        WaitingForOrderPickup, // Raise Hand
        WaitingForFood, // Idle
        Eating, // Eating
        WaitingForBillCheck, // Raise Hand
        Leave // Destroy
    }
    public CustomerState currState;

    public enum Food
    {
        Hamburger,
        Sandwich,
        Pizza,
        MushroomSoup
    }
    public Food foodType;

    public Animator animator;
    public int customerType;

    private float readTime = 3f;
    private float eatTime = 3f;
    private float patience = 5f;
    
    public bool isSeated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currState = CustomerState.InQueue;
        foodType = GetFood();
        StartCoroutine(StateMachine());
    }

    private static Food GetFood()
    {
        Food[] foodTypes = (Food[]) Enum.GetValues(typeof(Type));
        Food food = foodTypes[UnityEngine.Random.Range(0, foodTypes.Length)];
        return food;
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
                return "Customer_" + customerType + "_Read_Menu";
            case CustomerState.WaitingForOrderPickup:
                return "Customer_" + customerType + "_Raise_Hand";
            case CustomerState.WaitingForFood:
                return "Customer_" + customerType + "_Idle";
            case CustomerState.Eating:
                return "Customer_" + customerType + "_Eating";
            case CustomerState.WaitingForBillCheck:
                return "Customer_" + customerType + "_Raise_Hand";
            case CustomerState.Leave:
                return "Customer_" + customerType + "_Wave";
            default:
                return "Customer_" + customerType + "_Idle";
        }
    }

    private void PlayAnimation(CustomerState state)
    {
        animator.SetInteger("CustomerState", (int)state);
    }
    
    private IEnumerator InQueue()
    {
        PlayAnimation(CustomerState.InQueue);
        yield return PatienceTimer(patience, true, CustomerState.ReadingMenu);
    }

    private IEnumerator ReadMenu()
    {
        PlayAnimation(CustomerState.ReadingMenu);
        yield return PatienceTimer(readTime, false, CustomerState.WaitingForOrderPickup);
    }

    private IEnumerator WaitForOrderPickup()
    {
        PlayAnimation(CustomerState.WaitingForOrderPickup);
        yield return PatienceTimer(patience, true, CustomerState.WaitingForFood);
    }
    
    private IEnumerator WaitForFood()
    {
        PlayAnimation(CustomerState.WaitingForFood);
        yield return PatienceTimer(patience, true, CustomerState.Eating);
    }

    private IEnumerator Eating()
    {
        PlayAnimation(CustomerState.Eating);
        yield return PatienceTimer(eatTime, false, CustomerState.WaitingForBillCheck);
    }

    private IEnumerator WaitForBillCheck()
    {
        PlayAnimation(CustomerState.WaitingForBillCheck);
        yield return PatienceTimer(patience, true, CustomerState.Leave);
    }

    private void Leave()
    {
        PlayAnimation(CustomerState.Leave);
        Destroy(gameObject, 1f);
    }

    public void SetSeated()
    {
        isSeated = true;
        currState = CustomerState.ReadingMenu;
    }

    private IEnumerator PatienceTimer(float duration, bool leaveOnTimeout, CustomerState nextState)
    {
        while (duration > 0f)
        {
            if (currState == nextState)
            {
                yield break;
            }
            duration -= Time.deltaTime;
            yield return null;
        }

        if (leaveOnTimeout)
        {
            currState = CustomerState.Leave;
        }
        else
        {
            currState = nextState;
        }
    }

    public void Interact()
    {
        if (!PauseMenu.gameIsPaused)
        {
            switch(currState)
            {
                case CustomerState.WaitingForOrderPickup:
                    currState = CustomerState.WaitingForFood;
                    Debug.Log("Order picked up.");
                    break;
                case CustomerState.WaitingForFood:
                    currState = CustomerState.Eating;
                    Debug.Log("Food served.");
                    break;
                case CustomerState.WaitingForBillCheck:
                    currState = CustomerState.Leave;
                    Debug.Log("Bill checked.");
                    break;
            }
        }
    }
}
