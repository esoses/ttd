using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public GameObject towerToBuild;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;


            if (Physics.Raycast(ray, out raycastHit, 100, -5, QueryTriggerInteraction.Ignore)) 
            {
                if (raycastHit.transform.gameObject.name == "Grass")
                {
                    PlaceTower(towerToBuild, raycastHit.point + new Vector3(0,0.5f,0));
                }
                else if (raycastHit.transform.gameObject.tag == "Turret")
                {
                    Debug.Log("Turret was clicked!");
                }
            }
        }
    }

    public void PlaceTower(GameObject tower, Vector3 position)
    {      
        Instantiate(tower, position, Quaternion.identity);      
    }
}
