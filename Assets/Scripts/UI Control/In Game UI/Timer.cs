using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Sprite[] timerSprites;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite(int index)
    {
        if (index >= 0 && index < timerSprites.Length)
        {
            spriteRenderer.sprite = timerSprites[index];
        }
    }
}
