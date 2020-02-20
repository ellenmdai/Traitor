using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //current role
    public Role role;
    //role of the closest enemy; AKA next role would change into
    public Role closestRole;
    private List<GameObject> npcsInRange;

    //current Sprite
    public Sprite currSprite;
    Rigidbody2D rb;
    SpriteRenderer sr;

    //public coefficients; public for now for easy tuning in dev phase.
    public float SPEED = 0.1f;
    public Sprite starterSprite;

    //Sprites to change to

    public Sprite playerSprite;
    public Sprite janitorSprite;
    public Sprite guardSprite;
    public Sprite servantSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (starterSprite == null)
        {
            starterSprite = playerSprite;
        }
        if(sr.sprite == null)
        {
            sr.sprite = starterSprite;
        }
        role = Role.Player;
        closestRole = role;
        npcsInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //directional movement with a-s-d-w
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 tempVect = new Vector3(-1, 0, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            rb.MovePosition(transform.position + tempVect);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 tempVect = new Vector3(1, 0, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            rb.MovePosition(transform.position + tempVect);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 tempVect = new Vector3(0, 1, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            rb.MovePosition(transform.position + tempVect);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 tempVect = new Vector3(0, -1, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            rb.MovePosition(transform.position + tempVect);
        }
        if(npcsInRange.Count != 0)
        {
            findClosestRole();
        }

        //change player sprites to fit colors
        //can take out some colors or add refresh time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //sr.sprite = getSprite(closestRole);
            if (npcsInRange.Count != 0 && closestRole != role)
            {
                switchRole();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision with another NPC
        //check color to see if player reset
        if (collision.gameObject.GetComponent<NPC>())
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>())
        {
            GameObject otherNPC = collision.gameObject;
            npcsInRange.Add(otherNPC);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>())
        {
            GameObject otherNPC = collision.gameObject;
            npcsInRange.Remove(otherNPC);
        }
    }
    //do necessary changes when role is switched
    private void switchRole()
    {
        role = closestRole;
        sr.sprite = getSprite(role);
        // GetComponent<CapsuleCollider2D>().size = sr.sprite.bounds.size;
    }
    
    private void findClosestRole()
    {
        Vector3 playerPosition = transform.position;
        Vector3 closest = playerPosition;
        float closestDistance = 1000f;
        foreach(GameObject npc in npcsInRange)
        {
            Vector3 npcPosition = npc.transform.position;
            float npcDistance = Vector3.Distance(npcPosition, playerPosition);
            if(npcDistance <= closestDistance)
            {
                closestDistance = npcDistance;
                closest = npcPosition;
                closestRole = npc.GetComponent<NPC>().role;
            }

        }
    }
    private Sprite getSprite(Role role)
    {
        switch (role)
        {
            case Role.Player:
                return playerSprite;
            case Role.Guard:
                return guardSprite;
            case Role.Janitor:
                return janitorSprite;
            case Role.Servant:
                return servantSprite;
            default:
                return starterSprite;
        }
    }

}
