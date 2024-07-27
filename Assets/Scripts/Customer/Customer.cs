using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public event Action BurgerOrder;
    public event Action SandwichOrder;
    public event Action SaladOrder;
    public event Action ChickenSetOrder;
    public event Action LambSetOrder;

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
        Burger,
        ChickenSet,
        Salad,
        Sandwich,
        LambSet
    }
    public Food foodType;

    public Timer timer;

    public int tableId;
    public TextMeshPro tableIdtext;

    public Animator animator;
    public int customerType;

    private float readTime = 3f;
    private float eatTime = 3f;
    private float patience = 10f;
    
    public bool isSeated = false;
    public bool billChecked = false;

    public static Customer Instance
    {
        get; private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tableId = UnityEngine.Random.Range(0, 10);
        tableIdtext.text = tableId.ToString();
        tableIdtext.enabled = false;
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
        if (billChecked)
        {
            PlayAnimation(CustomerState.Leave);
        }
        else
        {
            PlayAnimation(CustomerState.InQueue);
        }

        yield return new WaitForSeconds(1f);

        if (isSeated)
        {
            Vector2Int seat = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            FindObjectOfType<SeatManager>().FreeUp(seat);
        }
        else
        {
            FindObjectOfType<QueueManager>().RemoveCustomer(gameObject);
        }        

        FindObjectOfType<LevelComplete>().remainingCustomers--;

        if (!billChecked)
        {
            FindObjectOfType<Health>().RemoveHealth();
        }

        Destroy(gameObject);
    }

    public void SetSeated()
    {
        isSeated = true;
        tableIdtext.enabled = true;
        currState = CustomerState.ReadingMenu;
    }

    private IEnumerator PatienceTimer(float duration, bool leaveOnTimeout, CustomerState nextState)
    {
        // Debug.Log("Patience Timer Started");
        timer.transform.gameObject.SetActive(leaveOnTimeout);

        float timePerSprite = duration / timer.timerSprites.Length;

        int currSpriteIndex = 0;
        timer.UpdateSprite(currSpriteIndex);
        float spriteTimer = timePerSprite;

        while (duration > 0f)
        {
            if (currState == nextState)
            {
                // Debug.Log("Patience Timer Stopped: State Changed");
                timer.transform.gameObject.SetActive(false);
                yield break;
            }
            duration -= Time.deltaTime;
            spriteTimer -= Time.deltaTime;

            if (spriteTimer <= 0f && currSpriteIndex < timer.timerSprites.Length)
            {
                currSpriteIndex++;
                timer.UpdateSprite(currSpriteIndex);
                // Debug.Log("Sprite Updated to: " + currSpriteIndex);
                spriteTimer = timePerSprite;
            }

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

        // Debug.Log("Patience Timer Ended");
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
            case Food.Burger:
                EventManager.TriggerBurgerOrder();
                Debug.Log("Picked up a Burger order.");
                break;
            case Food.ChickenSet:
                EventManager.TriggerChickenSetOrder();
                Debug.Log("Picked up a ChickenSet order.");
                break;
            case Food.Salad:
                EventManager.TriggerSaladOrder();
                Debug.Log("Picked up a Salad order.");
                break;
            case Food.Sandwich:
                EventManager.TriggerSandwichOrder();
                Debug.Log("Picked up a Sandwich order.");
                break;
            case Food.LambSet:
                EventManager.TriggerLambSetOrder();
                Debug.Log("Picked up a LambSet order.");
                break;
        }
        
        currState = CustomerState.WaitingForFood;
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
