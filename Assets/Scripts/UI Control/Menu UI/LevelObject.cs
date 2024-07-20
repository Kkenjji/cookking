using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    public Image[] stars;
    public Sprite emptyStar;
    public Sprite yellowStar;
    
    public void ActivateStars(int num)
    {
        if (num == 0)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].sprite = emptyStar;
            }
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                stars[i].sprite = yellowStar;
            }
        }
    }
}
