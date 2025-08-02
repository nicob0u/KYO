using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;


    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddScore(int points)
    {
        score += points;
        UnityEngine.Debug.Log($"Current score: {score}");

    }

    public int GetScore() => score;

}
