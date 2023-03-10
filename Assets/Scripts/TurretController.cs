using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurretController : MonoBehaviour
{
    public enum Targeting
    {
        First,
        Last,
        Strongest,
        Weakest,
        Fastest,
        Slowest,
        LeastHP,
        MostHP

    }

    public Targeting selectedTargeting;
    
    public float damage;
    public float shotInterval;
    public float timeUntilNextShot;

    private Transform gunBase;
    private List<GameObject> _enemiesInRange = new List<GameObject>();
    
    
    private void Start()
    {
        timeUntilNextShot = shotInterval;
        gunBase = transform.GetChild(0);
        //selectedTargeting = Targeting.First;

        StartCoroutine(WaitForEnemies());
    }

    
    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedObj = other.gameObject;

        if (collidedObj.CompareTag("Enemy"))
        {
            collidedObj.GetComponent<EnemyHealth>().OnEnemyDeath += RemoveDeadEnemy;
            _enemiesInRange.Add(collidedObj);
        }
    }
    
    
    private void OnTriggerExit(Collider other)
    {
        GameObject collidedObj = other.gameObject;

        if (collidedObj.CompareTag("Enemy"))
        {
            collidedObj.GetComponent<EnemyHealth>().OnEnemyDeath -= RemoveDeadEnemy;
            _enemiesInRange.Remove(collidedObj);
        }
    }

    
    private void RemoveDeadEnemy(GameObject enemy)
    {       
        _enemiesInRange.Remove(enemy);
    }

    
    private IEnumerator WaitForEnemies()
    {
        
        if(_enemiesInRange.Count > 0)
        {
            StartCoroutine(AttackEnemies());
            yield break;
        }

        timeUntilNextShot -= Time.deltaTime;

        yield return new WaitForEndOfFrame();
        StartCoroutine(WaitForEnemies());
    }

    
    private IEnumerator AttackEnemies()
    {
        
        if (_enemiesInRange.Count == 0)
        {
            StartCoroutine(WaitForEnemies());
            yield break;
        }

        
        if(FindEnemy(selectedTargeting) != null)
        {
            GameObject targetedEnemy = FindEnemy(selectedTargeting);
            FaceEnemy(targetedEnemy.transform);
            if (timeUntilNextShot <= 0)
            {
                ShootEnemy(targetedEnemy);
                timeUntilNextShot = shotInterval;
            }
        }
        
        timeUntilNextShot -= Time.deltaTime;

        yield return new WaitForEndOfFrame();
        StartCoroutine(AttackEnemies());
    }

    
    private GameObject FindEnemy(Targeting targeting)
    {
        if(targeting == Targeting.Fastest)
        {
            var enemies = _enemiesInRange.OrderBy(o => o.GetComponent<EnemyMovement>().speed).ToList();

            return enemies[0];
        }
        else if(targeting == Targeting.First)
        {
            return _enemiesInRange[0];
        }
        else return _enemiesInRange[^1];
    }

    
    private void FaceEnemy(Transform enemyTransform)
    {
        var direction = new Vector2(
            enemyTransform.transform.position.x - transform.position.x,
            enemyTransform.transform.position.z - transform.position.z
        );
        gunBase.transform.forward = direction;
    }

    
    private void ShootEnemy(GameObject enemy)
    {
        var enemyHealth =  enemy.GetComponent<EnemyHealth>();
        
        enemyHealth.currentHealth -= damage;
        if (enemyHealth.currentHealth <= 0) 
        {
            _enemiesInRange.Remove(enemy);
        }
        enemyHealth.CheckForDeath();
    }
}