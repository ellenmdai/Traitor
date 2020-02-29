using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NPC : MonoBehaviour
{

    
    bool enemyInSight;
    List<GameObject> enemies;
    
    
    public float speed;
    public Role role;

    public Transform viewPivot;


    //path
    public bool hasPathToFollow;
    [SerializeField]
    public Transform[] waypoints;
    private int waypointIndex=0;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyInSight = false;
        enemies = new List<GameObject>();

        //path
        if (hasPathToFollow)
        {
            transform.position = waypoints[waypointIndex].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPathToFollow)
        {
            //move
            if(waypointIndex < waypoints.Length)
            {
                rotateViewBasedOnVelocity(Vector3.MoveTowards(transform.position,
                                                         waypoints[waypointIndex].position,
                                                         speed * Time.deltaTime) - transform.position);
                transform.position = Vector3.MoveTowards(transform.position,
                                                         waypoints[waypointIndex].position,
                                                         speed * Time.deltaTime);

                if(transform.position == waypoints[waypointIndex].position)
                {
                    waypointIndex += 1;
                    if (waypointIndex == waypoints.Length) waypointIndex = 0;
                }
            }
        }
    }

    private void rotateViewBasedOnVelocity(Vector3 velocity)
    {
        float degreeAngleToDirection = Mathf.Atan2(velocity.y, velocity.x) * 180f / Mathf.PI;

        viewPivot.rotation = Quaternion.Euler(0, 0, degreeAngleToDirection);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("enter trigger");
        GameObject otherGO = collision.gameObject;
        if (otherGO.GetComponent<PlayerController>())
        {
            Role otherRole = otherGO.GetComponent<PlayerController>().role;
            if (otherRole == role || otherRole == Role.Player)
            {
                if (collision is CapsuleCollider2D)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
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
            Role otherRole = otherGO.GetComponent<PlayerController>().role;
            if (otherRole == role || otherRole == Role.Player)
            {
                if (collision is CapsuleCollider2D)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
        else if (otherGO.GetComponent<NPC>())
        {
            //probably don't need
        }

    }




}
