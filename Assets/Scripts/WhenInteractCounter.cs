using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenInteractCounter : MonoBehaviour
{
    [SerializeField] private EmptyCounter emptyCounter;
    [SerializeField] private GameObject visual;

    private void Start()//do on start and not awake to make sure Instance is initialised first
    {
        ChefController.Instance.SelectedCounter += Instance_SelectedCounter;
    }

    private void Instance_SelectedCounter(object sender, ChefController.SelectedCounterEventArgs e)
    {
        
        if (e.InteractedCounter == emptyCounter)
        {
            Show();
        }
        else {
            Hide();  
        }
    }

    private void Show() {
        
        visual.SetActive(true);
}

    private void Hide()
    {
        
        visual.SetActive(false);
    }
}
