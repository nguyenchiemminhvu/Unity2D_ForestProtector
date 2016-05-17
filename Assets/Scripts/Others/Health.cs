using UnityEngine;
using System.Collections;

public class Health {

    public int health;
    public int maxHealth;

    public Health(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public bool isDead()
    {
        return this.health <= 0;
    }

    public void increaseHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void decreaseHealth(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }
    }
}
