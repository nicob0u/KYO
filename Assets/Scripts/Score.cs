using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int points = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            points++;
            ScoreManager.Instance.AddScore(points);
            
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
