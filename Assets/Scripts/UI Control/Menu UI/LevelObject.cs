using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    public Image[] stars;
    
    public void ActivateStars(int num)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i == num - 1)
            {
                stars[i].enabled = true;
            }
            else
            {
                stars[i].enabled = false;
            }
        }
    }
}
