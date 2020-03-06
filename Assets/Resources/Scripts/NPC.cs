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

    
    public bool hasDirectionalView = false;
    public ViewDirection viewDirection = ViewDirection.Down;
    public Transform viewPivot;

    Animator animator;

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
        animator = GetComponent<Animator>();

        //path
        if (hasPathToFollow)
        {
            transform.position = waypoints[waypointIndex].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDirectionalView)
        {
            rotateViewBasedOnViewDirection();
            seeUsingRaycast();
        }
        
        if (hasPathToFollow)
        {
            //move
            if(waypointIndex < waypoints.Length)
            {
                rotateViewBasedOnVelocityAndUpdateViewDirection(Vector3.MoveTowards(transform.position,
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

    private void seeUsingRaycast()
    {
        RaycastHit2D[] hits1 = Physics2D.RaycastAll(viewPivot.position, -viewPivot.up, 2f);
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(viewPivot.position, -viewPivot.up + viewPivot.right, 1.414f);
        RaycastHit2D[] hits3 = Physics2D.RaycastAll(viewPivot.position, -viewPivot.up - viewPivot.right, 1.414f);

        Debug.DrawRay(viewPivot.position, (-viewPivot.up) * 2f);
        Debug.DrawRay(viewPivot.position, (-viewPivot.up + viewPivot.right) );
        Debug.DrawRay(viewPivot.position, (-viewPivot.up - viewPivot.right) );


        // the things that the ray runs into is in order of distance from NPC
        // therefore, hits[0] may be itself, also if if one of the hits is a
        // wall, I will stop looking for further hits after that because we
        // don't want the NPCs to see through walls

        // also I separated out each raycast because the lengths might not match

        for(int i=0; i<hits1.Length; i++)
        {
            GameObject thingHit = hits1[i].collider.gameObject;
            if (thingHit.layer == LayerMask.NameToLayer("Walls"))
            {
                break;
            }
            else if (thingHit.GetComponent<PlayerController>() && hits1[i].collider is CapsuleCollider2D) //hit player
            {
                NPCSeesPlayer(thingHit);
            }
        }
        for (int i = 0; i < hits2.Length; i++)
        {
            GameObject thingHit = hits2[i].collider.gameObject;
            if (thingHit.layer == LayerMask.NameToLayer("Walls"))
            {
                break;
            }
            else if (thingHit.GetComponent<PlayerController>() && hits2[i].collider is CapsuleCollider2D) //hit player
            {
                NPCSeesPlayer(thingHit);
            }
        } 
        for (int i = 0; i < hits3.Length; i++)
        {
            GameObject thingHit = hits3[i].collider.gameObject;
            if (thingHit.layer == LayerMask.NameToLayer("Walls"))
            {
                break;
            }
            else if (thingHit.GetComponent<PlayerController>() && hits3[i].collider is CapsuleCollider2D) //hit player
            {
                NPCSeesPlayer(thingHit);
            }
        }


    }

    private void NPCSeesPlayer(GameObject player)
    {
        if(player.GetComponent<PlayerController>().role == role || player.GetComponent<PlayerController>().role == Role.Player)
        {
            GameStats.LevelDeaths++;
            GameStats.TotalDeaths++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    private void rotateViewBasedOnVelocityAndUpdateViewDirection(Vector3 velocity)
    {
        float degreeAngleToDirection = Mathf.Atan2(velocity.y, velocity.x) * 180f / Mathf.PI;

        viewPivot.rotation = Quaternion.Euler(0, 0, degreeAngleToDirection);


        //update viewDirection variable

        if (degreeAngleToDirection < 0f)
        {
            degreeAngleToDirection += 360f;
        }
        else if (degreeAngleToDirection > 360f)
        {
            degreeAngleToDirection -= 360f;
        }

        if (degreeAngleToDirection < 45f || degreeAngleToDirection >= 315f)
        {
            viewDirection = ViewDirection.Right;
        }
        else if (degreeAngleToDirection < 135f && degreeAngleToDirection >= 45f)
        {
            viewDirection = ViewDirection.Up;
        }
        else if (degreeAngleToDirection < 225f && degreeAngleToDirection >= 135f)
        {
            viewDirection = ViewDirection.Left;
        }
        else if (degreeAngleToDirection < 315f && degreeAngleToDirection >= 225f)
        {
            viewDirection = ViewDirection.Down;
        }
        if (animator) { 
            animator.SetInteger("View Direction", (int)viewDirection);
        }

    }

    private void rotateViewBasedOnViewDirection()
    {
        if(viewDirection == ViewDirection.Right)
        {
            viewPivot.rotation = Quaternion.Euler(0, 0, 90f);
        }
        else if (viewDirection == ViewDirection.Up)
        {
            viewPivot.rotation = Quaternion.Euler(0, 0, 180f);
        }
        else if (viewDirection == ViewDirection.Left)
        {
            viewPivot.rotation = Quaternion.Euler(0, 0, 270f);
        }
        else //(viewDirection == ViewDirection.Down)
        {
            viewPivot.rotation = Quaternion.Euler(0, 0, 0f);
        }
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
