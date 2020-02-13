using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    bool enemyInSight;
    List<GameObject> enemies;

    
    public float speed;
    public Role role;
    public GameObject closestEnemy;

    public Transform viewPivot;


    // Start is called before the first frame update
    void Start()
    {
        enemyInSight = false;
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInSight)
        {
            //find closest enemy

            //first find this position
            Vector3 thisPos = transform.position;

            //go through all enemies
            float shortestDistance = 1000f;
            foreach(GameObject enemy in enemies)
            {
                float calculatedDistance = Vector3.Distance(thisPos, enemy.transform.position);
                if(calculatedDistance <= shortestDistance)
                {
                    shortestDistance = calculatedDistance;
                    closestEnemy = enemy;
                }
            }


            //move towards that enemy
            transform.position = Vector3.MoveTowards(thisPos, closestEnemy.transform.position, speed);

            Vector3 directionTowardsEnemy = transform.position - closestEnemy.transform.position;
            float degreeAngleToEnemy = Mathf.Atan2(directionTowardsEnemy.y, directionTowardsEnemy.x) * 180f / Mathf.PI;

            viewPivot.rotation = Quaternion.Euler(0,0,degreeAngleToEnemy);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("enter trigger");
        GameObject otherGO = collision.gameObject;
        if (otherGO.GetComponent<PlayerController>())
        {
            if (otherGO.GetComponent<PlayerController>().role == role)
            {
                enemies.Add(otherGO);
                enemyInSight = true;
            }
        }
        else if (otherGO.GetComponent<NPC>())
        {
            //probably don't need
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("stay trigger");
        GameObject otherGO = collision.gameObject;
        if (otherGO.GetComponent<PlayerController>())
        {
            if (otherGO.GetComponent<PlayerController>().role == role && !enemies.Contains(otherGO))
            {
                enemies.Add(otherGO);
                enemyInSight = true;
            }
            if (otherGO.GetComponent<PlayerController>().role != role && enemies.Contains(otherGO))
            {
                enemies.Remove(otherGO);
                if (enemies.Count == 0)
                {
                    enemyInSight = false;
                }
            }
        }
        else if (otherGO.GetComponent<NPC>())
        {
            //probably don't need
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("trigger exit");
        if (enemies.Contains(collision.gameObject))
        {
            enemies.Remove(collision.gameObject);
            if (enemies.Count == 0)
            {
                enemyInSight = false;
            }
        }
    }

    
}
