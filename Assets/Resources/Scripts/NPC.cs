using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    SpriteRenderer sr;
    bool enemyInSight;
    List<GameObject> enemies;

    public Sprite redPlayer;
    public Sprite bluePlayer;
    public Sprite greenPlayer;
    public Sprite yellowPlayer;
    public Sprite redNPC;
    public Sprite blueNPC;
    public Sprite greenNPC;
    public Sprite yellowNPC;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
            Vector3 closestEnemyPosition = transform.position;
            foreach(GameObject enemy in enemies)
            {
                float calculatedDistance = Vector3.Distance(thisPos, enemy.transform.position);
                if(calculatedDistance <= shortestDistance)
                {
                    shortestDistance = calculatedDistance;
                    closestEnemyPosition = enemy.transform.position;
                }
            }


            //move towards that enemy
            transform.position = Vector3.MoveTowards(thisPos, closestEnemyPosition, speed);
            //print(closestEnemyPosition.ToString());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("enter trigger");
        SpriteRenderer otherSR = collision.gameObject.GetComponent<SpriteRenderer>();
        Sprite otherTeam = otherSR.sprite;
        Sprite thisTeam = sr.sprite;

        if(thisTeam == redNPC && !(otherTeam == redNPC || otherTeam == redPlayer))
        {
            enemyInSight = true;
            if (!enemies.Contains(collision.gameObject))
            {
                enemies.Add(collision.gameObject);
            }
        }
        if (thisTeam == blueNPC && !(otherTeam == blueNPC || otherTeam == bluePlayer))
        {
            enemyInSight = true;
            if (!enemies.Contains(collision.gameObject))
            {
                enemies.Add(collision.gameObject);
            }
        }
        if (thisTeam == greenNPC && !(otherTeam == greenNPC || otherTeam == greenPlayer))
        {
            enemyInSight = true;
            if (!enemies.Contains(collision.gameObject))
            {
                enemies.Add(collision.gameObject);
            }
        }
        if (thisTeam == yellowNPC && !(otherTeam == yellowNPC || otherTeam == yellowPlayer))
        {
            enemyInSight = true;
            if (!enemies.Contains(collision.gameObject))
            {
                enemies.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("trigger exit");
        enemies.Remove(collision.gameObject);
        if(enemies.Count == 0)
        {
            enemyInSight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Sprite thisTeam = sr.sprite;
        Sprite otherTeam = collision.gameObject.GetComponent<SpriteRenderer>().sprite;

        if (thisTeam == redNPC && !(otherTeam == redNPC || otherTeam == redPlayer))
        {
            Destroy(gameObject);   
        }
        if (thisTeam == blueNPC && !(otherTeam == blueNPC || otherTeam == bluePlayer))
        {
            Destroy(gameObject);
        }
        if (thisTeam == greenNPC && !(otherTeam == greenNPC || otherTeam == greenPlayer))
        {
            Destroy(gameObject);
        }
        if (thisTeam == yellowNPC && !(otherTeam == yellowNPC || otherTeam == yellowPlayer))
        {
            Destroy(gameObject);
        }
    }
}
