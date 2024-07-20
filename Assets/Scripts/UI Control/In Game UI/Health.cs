using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int heartCount;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        if (heartCount > 5)
        {
            Debug.Log("Error: HeartCount is greater than 5!");
        }

        health = heartCount;
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= hearts.Length - heartCount)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void AddHealth()
    {
        if (health + 1 <= heartCount)
        {
            health += 1;
            UpdateHealth();
        }
    }

    public void RemoveHealth()
    {
        health -= 1;
        UpdateHealth();
        if (health == 0)
        {
            // GameOver
        }
    }

    private void UpdateHealth()
    {
        int temp = health;
        for (int i = hearts.Length - heartCount; i < hearts.Length; i++)
        {
            if (temp > 0)
            {
                hearts[i].sprite = fullHeart;
                temp--;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
