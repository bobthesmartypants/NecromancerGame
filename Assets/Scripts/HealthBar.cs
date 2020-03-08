using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int maxHearts = 10;
    private int maxHealth;
    public Image[] heartImages;
    public Sprite[] heartSprites;

    public int healthColor;

    // Start is called before the first frame update
    void Start()
    {
        setMaxHealth(maxHearts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMaxHealth(int max){
        maxHealth = max;
        for (int i = 0; i < maxHearts; i++){
            if (maxHealth <= i){
                heartImages[i].enabled = false;
            } else{
                heartImages[i].enabled = true;
                heartImages[i].sprite = heartSprites[2*healthColor];
            }
        }
    }

    public void setCurrentHealth(int currentHealth){
        for (int i = 0; i < maxHealth; i++){
            if (currentHealth <= i){
                heartImages[i].sprite = heartSprites[2*healthColor];
            } else
            {
                heartImages[i].sprite = heartSprites[2*healthColor+1];
            }
        }
    }
}
