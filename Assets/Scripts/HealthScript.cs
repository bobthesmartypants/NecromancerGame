using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript
{
    public int startingHealth = 10;
    public int maxHealth = 10;
    private int currentHealth;

    HealthScript(int startingHealth, int maxHealth)
    {
        if (maxHealth < startingHealth)
        {
            startingHealth = maxHealth;
        }
        
        currentHealth = startingHealth;
    }

    // change health back to max value
    public void setHealthToMax()
    {
        currentHealth = maxHealth;
    }

    // decrease health by one. Returns false if character is dead, true if character is alive
    public bool decrementHealth()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            return false;
        }
        return true;
    }
}
