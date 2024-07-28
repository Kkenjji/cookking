using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public GameObject winUI;
    public GameObject gameOverUI;
    public GameObject gameElementsUI;
    public Health healthTracker;
    public int targetProfits;
    public int remainingCustomers;
    public bool hasEnded;
    
    // Start is called before the first frame update
    void Start()
    {
        healthTracker = FindObjectOfType<Health>();
        remainingCustomers = FindObjectOfType<Spawner>().totalCustomers;
        hasEnded = false;
        remainingCustomers = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEnded)
        {
            if (healthTracker.health == 0)
            {
                FailLevel();
            }
            else if (remainingCustomers == 0)
            {
                int stars = 1;

                if (FindObjectOfType<Profits>().total >= targetProfits)
                {
                    stars++;
                }

                if (healthTracker.health == healthTracker.heartCount)
                {
                    stars++;
                }

                StartCoroutine(CompleteLevel(stars));
            }
        }
    }

    private void FailLevel()
    {
        if (hasEnded)
        {
            return;
        }
        hasEnded = true;
        FindObjectOfType<PauseMenu>().FreezeGame();
        gameOverUI.SetActive(true);
        Complete(0);
    }

    private IEnumerator CompleteLevel(int stars)
    {
        if (hasEnded)
        {
            yield break;
        }
        yield return new WaitForSeconds(1f);
        hasEnded = true;
        FindObjectOfType<PauseMenu>().FreezeGame();
        winUI.SetActive(true);
        Complete(stars);
    }

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
    }
}
