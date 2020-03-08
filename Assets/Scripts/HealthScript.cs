using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int startingHealth = 10;
    public int maxHealth = 10;
    private int currentHealth;
    private bool alive = true;


    // Start is called before the first frame update
    void Start()
    {
        if (maxHealth < startingHealth)
        {
            startingHealth = maxHealth;
        }
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            die();
        }
    }

    // change health back to max value
    public void setHealthToMax()
    {
        currentHealth = maxHealth;
    }

    // decrease health by one
    public void decrementHealth()
    {
        currentHealth--;
    }

    private void die()
    {
        alive = false;
    }
}
