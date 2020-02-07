using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D _rb;
    SpriteRenderer sr;

    public Sprite redPlayer;
    public Sprite bluePlayer;
    public Sprite greenPlayer;
    public Sprite yellowPlayer;
    public Sprite redNPC;
    public Sprite blueNPC;
    public Sprite greenNPC;
    public Sprite yellowNPC;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if(sr.sprite == null)
        {
            sr.sprite = redPlayer;
        }
    }

    // Update is called once per frame
    void Update()
    {


        //directional movement with a-s-d-w
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddForce(Vector2.left * 12f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rb.AddForce(Vector2.right * 12f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _rb.AddForce(Vector2.up * 12f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rb.AddForce(Vector2.down * 12f);
        }




        //change player sprites to fit colors
        //can take out some colors or add refresh time
        if (Input.GetKey(KeyCode.U) && sr.sprite != redPlayer)
        {
            sr.sprite = redPlayer;
        }
        if (Input.GetKey(KeyCode.I) && sr.sprite != bluePlayer)
        {
            sr.sprite = bluePlayer;
        }
        if (Input.GetKey(KeyCode.O) && sr.sprite != greenPlayer)
        {
            sr.sprite = greenPlayer;
        }
        if (Input.GetKey(KeyCode.P) && sr.sprite != yellowPlayer)
        {
            sr.sprite = yellowPlayer;
        }




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision with another NPC
        //check color to see if player reset

        SpriteRenderer otherSR = collision.gameObject.GetComponent<SpriteRenderer>();
        Sprite otherTeam = otherSR.sprite;
        Sprite playerTeam = sr.sprite;

        if(otherTeam == redNPC && playerTeam != redPlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (otherTeam == blueNPC && playerTeam != bluePlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (otherTeam == greenNPC && playerTeam != greenPlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (otherTeam == yellowNPC && playerTeam != yellowPlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
