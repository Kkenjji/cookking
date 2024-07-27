using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public event Action BurgerOrder;
    public event Action SandwichOrder;
    public event Action SaladOrder;
    public event Action ChickenSetOrder;
    public event Action LambSetOrder;
    public static Customer Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance= this;
    }


    public enum CustomerState
    {
        InQueue, // Idle
        ReadingMenu, // Read Menu
        WaitingForOrderPickup, // Raise Hand
        WaitingForFood, // Idle
        Eating, // Eating
        WaitingForBillCheck, // Raise Hand
        Leave // Wave/Destroy
    }
    public CustomerState currState;

    public enum Food
    {
        FullBurger,
        ChickenSet,
        Salad,
        Sandwich,
        LambSet
    }
    public Food foodType;

    public int tableId;

    private Order myOrder;

    public Animator animator;
    public int customerType;

    private float readTime = 3f;
    private float eatTime = 3f;
    private float patience = 10f;
    
    public bool isSeated = false;
    public bool billChecked = false;

    // Start is called before the first frame update
    void Start()
    {
        tableId = UnityEngine.Random.Range(0, 10);
        animator = GetComponent<Animator>();
        currState = CustomerState.InQueue;
        foodType = GetFood();
        StartCoroutine(StateMachine());
    }

    private static Food GetFood()
    {
        Food[] foodTypes = (Food[]) Enum.GetValues(typeof(Food));
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

        StartCoroutine(Leave());
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

    private IEnumerator Leave()
    {
        if (isSeated)
        {
            Vector2Int seat = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            FindObjectOfType<SeatManager>().FreeUp(seat);
        }
        else
        {
            FindObjectOfType<QueueManager>().RemoveCustomer(gameObject);
        }

        PlayAnimation(CustomerState.Leave);

        FindObjectOfType<LevelComplete>().remainingCustomers--;
        yield return new WaitForSeconds(1f);

        if (!billChecked)
        {
            FindObjectOfType<Health>().RemoveHealth();
        }

        Destroy(gameObject);
        
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
                    PickUpOrder();
                    break;
                case CustomerState.WaitingForFood:
                    ServeFood();
                    break;
                case CustomerState.WaitingForBillCheck:
                    CheckBill();
                    break;
            }
        }
    }

    private void PickUpOrder()
    {
        FindObjectOfType<OrderQueue>().AddOrder(this.tableId, this.foodType);
        
        switch (foodType)
        {
            case Food.FullBurger:
                EventManager.TriggerBurgerOrder();
                Debug.Log("burgerorder");
                
                break;
            case Food.ChickenSet:
                EventManager.TriggerChickenSetOrder();
                Debug.Log("Chickenorder");
                
                break;
            case Food.Salad:
                EventManager.TriggerSaladOrder();
                Debug.Log("saladorder");
                
                break;
            case Food.Sandwich:
                EventManager.TriggerSandwichOrder();
                Debug.Log("sandwichorder");
                
                break;
            case Food.LambSet:
                EventManager.TriggerLambSetOrder();
                Debug.Log("lamborder");
                
                break;
        }
        
        currState = CustomerState.WaitingForFood;
        Debug.Log("Order picked up.");
    }

    private void ServeFood()
    {
        // WaiterInventory waiterInventory = FindObjectOfType<WaiterInventory>();
        // if (waiterInventory.currentFoodType == this.foodType)
        // {
            FindObjectOfType<OrderQueue>().RemoveOrder(this.tableId);
            currState = CustomerState.Eating;
            Debug.Log("Food served.");
        // }
    }

    private void CheckBill()
    {
        FindObjectOfType<Profits>().AddProfits(50);
        billChecked = true;
        currState = CustomerState.Leave;
        Debug.Log("Bill checked.");
    }    
}
