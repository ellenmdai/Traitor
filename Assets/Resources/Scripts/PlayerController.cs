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

    //current Sprite
    public Sprite currSprite;
    Rigidbody2D rb;
    SpriteRenderer sr;
    
    //public coefficients; public for now for easy tuning in dev phase.
    public float SPEED = 5f;
    public float RADIUS = 5f;
    public Sprite starterSprite;

    //Sprites to change to
    public Sprite janitorSprite;
    public Sprite guardSprite;
    public Sprite servantSprite;

    //role iterator
    private int roleIter;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        roleIter = 0;
        if(sr.sprite == null)
        {
            sr.sprite = starterSprite;
        }
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

        //change player sprites to fit colors
        //can take out some colors or add refresh time
        if (Input.GetKey(KeyCode.Space))
        {
            closestRole = (Role)roleIter; //temporary: able to iterate through roles
            updateRoleAndSprite();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision with another NPC
        //check color to see if player reset

        SpriteRenderer otherSR = collision.gameObject.GetComponent<SpriteRenderer>();
        Sprite otherTeam = otherSR.sprite;
        Sprite playerTeam = sr.sprite;
        print("HALKJ");
        //if(otherTeam == redNPC && playerTeam != redPlayer)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
        //if (otherTeam == blueNPC && playerTeam != bluePlayer)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
        //if (otherTeam == greenNPC && playerTeam != greenPlayer)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
        //if (otherTeam == yellowNPC && playerTeam != yellowPlayer)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }
    //do necessary changes when role is switched
    private void updateRoleAndSprite()
    {
        role = closestRole;
        sr.sprite = getSprite(role);
        GetComponent<CapsuleCollider2D>().size = sr.sprite.bounds.size;
    }
    //Todo: implement this method: every time an enemy entered the radius / exit the radius, calculate which is the closeset and return its role
    private void updateClosestEnemyRole()
    {
        
    }
    private Sprite getSprite(Role role)
    {
        switch (role)
        {
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
