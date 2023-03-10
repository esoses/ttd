using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Vector3 spawnPlace;

    public int amountToSpawn = 1;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                Instantiate(enemyToSpawn, spawnPlace, Quaternion.identity);
            }
            
        }
    }
}
