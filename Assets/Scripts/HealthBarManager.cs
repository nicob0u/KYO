using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Sprite[] hpSprites;
    public Image healthBarImg;
    public Health health;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SetHealth();
    }

    void SetHealth()
    {
        switch (health.currentHP)
        {
            case 0:
                healthBarImg.enabled = false;
                break;
            case 1:
                healthBarImg.sprite = hpSprites[0];
                break;
            case 2:
                healthBarImg.sprite = hpSprites[1];
                break;
            case 3:
                healthBarImg.sprite = hpSprites[2];
                break;
            case 4:
                healthBarImg.sprite = hpSprites[3];
                break;
            case 5:
                healthBarImg.sprite = hpSprites[4];
                break;
        }

    }

}
