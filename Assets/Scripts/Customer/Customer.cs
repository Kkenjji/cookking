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
    public event Action SteakOrder;

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
    public Food foodType;
    public GameObject myFood;
    public FoodTransferManager ftm;
    public Transform foodHold;

    public Timer timer;
    public int tableId;
    public TextMeshPro tableIdtext;

    public Animator animator;
    public int customerType;

    private float readTime;
    private float eatTime;
    private float patience;
    
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
        SetUp();
        ftm = FindObjectOfType<FoodTransferManager>();
        foodType = GetFood();
        myFood = Instantiate(ftm.foods[(int)foodType], foodHold);
        myFood.SetActive(false);
        tableId = FindObjectOfType<Spawner>().GetTableNumber();
        tableIdtext.text = tableId.ToString();
        tableIdtext.enabled = false;
        animator = GetComponent<Animator>();
        currState = CustomerState.InQueue;
        StartCoroutine(StateMachine());
    }

    private void SetUp()
    {
        LevelProperties LP = FindObjectOfType<LevelProperties>();
        this.readTime = LP.readTime;
        this.eatTime = LP.eatTime;
        this.patience = LP.patience;
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
        myFood.SetActive(true);
        PlayAnimation(CustomerState.Eating);
        yield return PatienceTimer(eatTime, false, CustomerState.WaitingForBillCheck);
        myFood.SetActive(false);
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
            PlayAnimation(CustomerState.InQueue); // play idle animation
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

        FindObjectOfType<OrderQueue>().RemoveOrder(tableId);

        if (!billChecked)
        {
            FindObjectOfType<Health>().RemoveHealth();
        }

        FindObjectOfType<Spawner>().SetAvailable(tableId);
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

        float timePerSprite = (duration - 1) / (timer.timerSprites.Length - 1);

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
            case Food.Steak:
                EventManager.TriggerSteakOrder();
                Debug.Log("Picked up a Steak order.");
                break;
        }
        
        currState = CustomerState.WaitingForFood;
    }

    private void ServeFood()
    {
        WaiterInventory waiterInventory = FindObjectOfType<WaiterInventory>();
        if (waiterInventory.hasItem && waiterInventory.foodType == this.foodType)
        {
            FindObjectOfType<OrderQueue>().RemoveOrder(this.tableId);
            waiterInventory.DiscardItem();
            currState = CustomerState.Eating;
            Debug.Log("Food served.");
        }
    }

    private void CheckBill()
    {
        FindObjectOfType<Profits>().AddProfits(100);
        billChecked = true;
        currState = CustomerState.Leave;
        Debug.Log("Bill checked.");
    }    
}
