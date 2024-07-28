using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Profits : MonoBehaviour
{
    public int total;
    public TMP_Text profitsText;

    // Start is called before the first frame update
    void Start()
    {
        total = 0;
        UpdateTotal();
    }

    public void AddProfits(int amount)
    {
        total += amount;
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        profitsText.text = total.ToString();
    }

    public void Penalty()
    {
        total -= 50;
        UpdateTotal();
    }
}
