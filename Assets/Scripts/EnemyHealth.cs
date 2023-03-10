using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;

    public Action<GameObject> death;

    private void Start()
    {
        currentHealth = health;
    }
    public void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            death.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
