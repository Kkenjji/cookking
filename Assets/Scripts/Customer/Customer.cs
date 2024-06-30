using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public bool isSeated = false;
    private float readTime;
    private float eatTime;
    public CustomerState currState;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //patience = Random.Range(patienceMin, patienceMax + 1);
        StartCoroutine(StateMachine());
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
        String animation = GetAnimation(CustomerState.ReadingMenu);
        animator.Play(animation);
        yield return new WaitForSeconds(readTime);
        animator.Play("RaiseHand");
    }

    private IEnumerator WaitForOrderPickup()
    {
        yield return new Vector2();
    }
    
    private IEnumerator WaitForFood()
    {
        yield return new Vector2();
    }

    private IEnumerator Eating()
    {
        yield return new Vector2();
    }

    private IEnumerator WaitForBillCheck()
    {
        yield return new Vector2();
    }

    private void Leave()
    {
        ;
    }
}
