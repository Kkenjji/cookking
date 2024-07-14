using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Profits : MonoBehaviour
{
    public int total;
    public TextMeshPro profitsText;

    // Start is called before the first frame update
    void Start()
    {
        total = 0;
        UpdateTotalUI();
    }

    public void AddProfits(int amount)
    {
        this.total += amount;
        UpdateTotalUI();
    }

    private void UpdateTotalUI()
    {
        profitsText.text = "$" + total.ToString();
    }
}
