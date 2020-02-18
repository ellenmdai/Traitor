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
    public float SPEED = 0.1f;
    public Sprite starterSprite;

    //Sprites to change to
    private Sprite janitorSprite;
    private Sprite guardSprite;
    private Sprite servantSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (starterSprite == null)
        {
            starterSprite = janitorSprite;
        }
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
            sr.sprite = getSprite(closestRole);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision with another NPC
        //check color to see if player reset

        SpriteRenderer otherSR = collision.gameObject.GetComponent<SpriteRenderer>();
        Sprite otherTeam = otherSR.sprite;
        Sprite playerTeam = sr.sprite;

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
    private void switchRole()
    {
        role = closestRole;
        sr.sprite = getSprite(role);
        GetComponent<CapsuleCollider2D>().size = sr.sprite.bounds.size;
    }
    //Todo: implement this method
    private Role getClosestEnemyRole()
    {
        return Role.Guard;
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
