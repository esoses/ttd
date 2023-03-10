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

    private Transform gunBase;

    private List<GameObject> EnemiesInRange = new List<GameObject>();
    
    public Targeting selectedTargeting;


    public float damage;
    
    public float shotInterval;
    public float timeUntillNextShot;

    private void Start()
    {
        timeUntillNextShot = shotInterval;
        gunBase = transform.GetChild(0);
        //selectedTargeting = Targeting.First;

        StartCoroutine(WaitForEnemies());
    }

    

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedObj = other.gameObject;

        if (collidedObj.tag == "Enemy")
        {
            collidedObj.GetComponent<EnemyHealth>().death += RemoveDeadEnemy;
            EnemiesInRange.Add(collidedObj);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject collidedObj = other.gameObject;

        if (collidedObj.tag == "Enemy")
        {
            collidedObj.GetComponent<EnemyHealth>().death -= RemoveDeadEnemy;
            EnemiesInRange.Remove(collidedObj);
        }
    }

    private void RemoveDeadEnemy(GameObject enemy)
    {       
        EnemiesInRange.Remove(enemy);
    }

    private IEnumerator WaitForEnemies()
    {
        
        if(EnemiesInRange.Count > 0)
        {
            StartCoroutine(AttackEnemies());
            yield break;
        }

        timeUntillNextShot -= Time.deltaTime;

        yield return new WaitForEndOfFrame();
        StartCoroutine(WaitForEnemies());
    }

    private IEnumerator AttackEnemies()
    {
        
        if (EnemiesInRange.Count == 0)
        {
            StartCoroutine(WaitForEnemies());
            yield break;
        }

        
        if(FindEnemy(selectedTargeting) != null)
        {
            GameObject targetedEnemy = FindEnemy(selectedTargeting);
            FaceEnemy(targetedEnemy.transform);
            if (timeUntillNextShot <= 0)
            {
                ShootEnemy(targetedEnemy);
                timeUntillNextShot = shotInterval;
            }
        }
        
        timeUntillNextShot -= Time.deltaTime;

        yield return new WaitForEndOfFrame();
        StartCoroutine(AttackEnemies());
    }

    private GameObject FindEnemy(Targeting targeting)
    {
        if(targeting == Targeting.Fastest)
        {
            List<GameObject> enemies = EnemiesInRange.OrderBy(o => o.GetComponent<EnemyMovement>().speed).ToList();

            return enemies[0];
        }
        else if(targeting == Targeting.First)
        {
            return EnemiesInRange[0];
        }
        else return EnemiesInRange[EnemiesInRange.Count - 1];
    }

    private void FaceEnemy(Transform enemyTransform)
    {
        Vector2 direction = new Vector2(
            enemyTransform.transform.position.x - transform.position.x,
            enemyTransform.transform.position.z - transform.position.z
        );
        gunBase.transform.forward = direction;
    }

    private void ShootEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyHealth>().currentHealth -= damage;
        if (enemy.GetComponent<EnemyHealth>().currentHealth <= 0) EnemiesInRange.Remove(enemy);
        enemy.GetComponent<EnemyHealth>().CheckForDeath();
        
    }
}
