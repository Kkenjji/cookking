using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    public Image foodImage;
    public TMP_Text tableIdText;
    public int tableId;

    public void Setup(int tableId, Sprite foodImage)
    {
        this.tableId = tableId;
        this.foodImage.sprite = foodImage;
        UpdateText();
    }

    private void UpdateText()
    {
        tableIdText.text = tableId.ToString();
    }
}
