using UnityEngine;
using System.Collections;


public class Score {

    public static int score;

    public Score()
    {
        score = PlayerPrefs.GetInt("curScore");
    }

    public static void addScore(int amount)
    {
        score += amount;
    }

    public static void resetScore()
    {
        score = 0;
    }
}
