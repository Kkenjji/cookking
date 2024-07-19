using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public void Complete(int starCount)
    {
        int currLevel = SceneManager.GetActiveScene().buildIndex;
        
        if (starCount != 0) // checks whether to unlock the next level
        {
            if (currLevel >= PlayerPrefs.GetInt("levelsUnlocked"))
            {
                PlayerPrefs.SetInt("levelsUnlocked", currLevel + 1);
            }

            Debug.Log("Level " + PlayerPrefs.GetInt("levelsUnlocked") + " Unlocked");
        }
        
        if (starCount > PlayerPrefs.GetInt("Level " + currLevel + " StarCount", 0)) // checks the highest starCount for the level
        {
            PlayerPrefs.SetInt("Level " + currLevel + " StarCount", starCount);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
