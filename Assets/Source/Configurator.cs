using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configurator : MonoBehaviour
{
    public int currentLevel = 1;

    public int[] levelTimers = { 4, 6, 7 };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public string GetCurrentLevel()
    {
        return currentLevel.ToString();
    }

    public int GetCurrentTimer()
    {
        return levelTimers[currentLevel - 1];
    }

    public void IncreaseLevel()
    {
        if (currentLevel < 3)
        {
            currentLevel++;
        }
    }

    public void SetCurrentLevel(int _lvl)
    {
        if (_lvl > 3)
        {
            currentLevel = 3;
        }
        else
        {
            currentLevel = _lvl;
        }
    }
}
