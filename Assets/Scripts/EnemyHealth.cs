using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;

    public Action<GameObject> OnEnemyDeath;

    private void Start()
    {
        currentHealth = health;
    }
    public void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
