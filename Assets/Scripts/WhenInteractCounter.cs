using UnityEngine;

public class WhenInteractCounter : Base
{
    [SerializeField] private Base baseCounter;
    [SerializeField] private GameObject visual;

    private void Start()//do on start and not awake to make sure Instance is initialised first
    {
        ChefController.Instance.SelectedCounter += Instance_SelectedCounter;
    }

    private void Instance_SelectedCounter(Base counter)
    {
        
        if (counter == baseCounter)
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
