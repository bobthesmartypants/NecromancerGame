using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript
{
    public int startingHealth = 10;
    public int maxHealth = 10;
    private int currentHealth;

    public HealthScript(int startingHealth, int maxHealth)
    {
        if (maxHealth < startingHealth)
        {
            startingHealth = maxHealth;
        }
        currentHealth = startingHealth;
    }

    // change health back to max value
    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
    }

    // decrease health by one. Returns false if character is dead, true if character is alive
    public bool DecrementHealth(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
