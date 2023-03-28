using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs 
{
   public static int bestScore
    {
        set
        {
            int oldScore = PlayerPrefs.GetInt(PrefsKey.BestScore.ToString(), 0);
            if(oldScore ==0 || oldScore < value)
            {
                PlayerPrefs.SetInt(PrefsKey.BestScore.ToString(), value);
            }
        }
        get => PlayerPrefs.GetInt(PrefsKey.BestScore.ToString(), 0);
    }
}
