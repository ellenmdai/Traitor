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


    //path
    public bool hasPathToFollow;
    [SerializeField]
    public Transform[] routes;
    private int routeToGo;
    private float tParam;
    private Vector3 NPCPositionOnPath;
    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        enemyInSight = false;
        enemies = new List<GameObject>();

        //path
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInSight)
        {
            ////find closest enemy

            ////first find this position
            //Vector3 thisPos = transform.position;

            ////go through all enemies
            //float shortestDistance = 1000f;
            //foreach(GameObject enemy in enemies)
            //{
            //    float calculatedDistance = Vector3.Distance(thisPos, enemy.transform.position);
            //    if(calculatedDistance <= shortestDistance)
            //    {
            //        shortestDistance = calculatedDistance;
            //        closestEnemy = enemy;
            //    }
            //}


            ////move towards that enemy
            //transform.position = Vector3.MoveTowards(thisPos, closestEnemy.transform.position, speed);

            //Vector3 directionTowardsEnemy = transform.position - closestEnemy.transform.position;
            //float degreeAngleToEnemy = Mathf.Atan2(directionTowardsEnemy.y, directionTowardsEnemy.x) * 180f / Mathf.PI;

            //viewPivot.rotation = Quaternion.Euler(0,0,degreeAngleToEnemy);
            
        }
        else if(hasPathToFollow)
        {
            //follow path
            if (coroutineAllowed)
            {
                StartCoroutine(GoByTheRoute(routeToGo));
            }
        }
    }

    private void rotateViewBasedOnVelocity(Vector3 velocity)
    {
        float degreeAngleToDirection = Mathf.Atan2(velocity.y, velocity.x) * 180f / Mathf.PI;

        viewPivot.rotation = Quaternion.Euler(0, 0, degreeAngleToDirection-180f);
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;
         
        Vector3 p0 = routes[routeNumber].GetChild(0).position;
        Vector3 p1 = routes[routeNumber].GetChild(1).position;
        Vector3 p2 = routes[routeNumber].GetChild(2).position;
        Vector3 p3 = routes[routeNumber].GetChild(3).position;

        Vector3 velocity = transform.position;

        while (tParam < 1 )
        {
            tParam += Time.deltaTime * speed;

            NPCPositionOnPath = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * tParam * Mathf.Pow(1 - tParam, 2) * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            velocity = transform.position - NPCPositionOnPath;

            rotateViewBasedOnVelocity(velocity);

            transform.position = NPCPositionOnPath;

            yield return new WaitForEndOfFrame();
        }


        tParam = 0f;

        routeToGo += 1;

        if(routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;

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
