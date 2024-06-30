using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        InQueue, // Idle
        ReadingMenu, // ReadMenu
        WaitingForOrderPickup, // RaiseHand
        WaitingForFood, // Idle
        Eating, // Eating
        WaitingForBillCheck, // RaiseHand
        Leave // Destroy
    }
    public Animator animator;
    public int customerType;

    private float patience;
    private int patienceMin = 20;
    private int patienceMax = 30;

    private float readTime;
    private float eatTime;
    public CustomerState currState;
    [SerializeField] bool isActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // animator = GetComponent<Animator>();
        patience = Random.Range(patienceMin, patienceMax + 1);
        // StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (currState != CustomerState.Leave)
        {
            switch (currState)
            {
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

    private IEnumerator ReadMenu()
    {
        string animation = GetAnimation(CustomerState.ReadingMenu);
        animator.Play(animation);
        SetActive(false);
        yield return new WaitForSeconds(readTime);
        currState = CustomerState.WaitingForOrderPickup;
    }

    private IEnumerator WaitForOrderPickup()
    {
        string animation = GetAnimation(CustomerState.WaitingForOrderPickup);
        animator.Play(animation);
        SetActive(true);
        yield return new WaitForSeconds(patience);
    }
    
    private IEnumerator WaitForFood()
    {
        string animation = GetAnimation(CustomerState.WaitingForFood);
        animator.Play(animation);
        SetActive(true);
        yield return new WaitForSeconds(patience);
    }

    private IEnumerator Eating()
    {
        string animation = GetAnimation(CustomerState.Eating);
        animator.Play(animation);
        SetActive(false);
        yield return new WaitForSeconds(eatTime);
    }

    private IEnumerator WaitForBillCheck()
    {
        string animation = GetAnimation(CustomerState.WaitingForBillCheck);
        animator.Play(animation);
        SetActive(true);
        yield return new WaitForSeconds(patience);
    }

    private void Leave()
    {
        string animation = GetAnimation(CustomerState.Leave);
        animator.Play(animation); // wave animation
        SetActive(false);
        Destroy(gameObject, 1f);
    }

    private void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void Interact()
    {
        Debug.Log(isActive + "Interacted");
        if (isActive == false)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
        return;
    }
}
