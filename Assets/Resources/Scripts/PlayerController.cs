using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    [SerializeField] public FieldOfView fieldOfView;
    //current role
    public Role role;
    //role of the closest enemy; AKA next role would change into
    public Role closestRole;
    private List<GameObject> npcsInRange;
    private bool canChangeRole; // if cooldown is elapsed and player can change roles again

    public Animator animator;
    public ViewDirection viewDirection = ViewDirection.Up;

    //current Sprite
    public Sprite currSprite;
    Rigidbody2D rb;
    SpriteRenderer sr;
    
    //public coefficients; public for now for easy tuning in dev phase.
    public float SPEED = 5f;
    public float RADIUS = 5f;
    public Sprite starterSprite;

    //Sprites to change to

    public Sprite playerSprite;

    public Sprite janitorSprite;
    public Sprite guardSprite;
    public Sprite servantSprite;
    public Sprite nobleSprite;

    //role iterator
    private int roleIter;

    //
    public UnityEvent ChangeRoleEvent; // notify timers to count down

    public bool GameComplete = false;

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
        canChangeRole = true;

        //viewDirection = ViewDirection.Up;

        animator = GetComponent<Animator>();

        if (animator)
        {
            animator.SetBool("isMoving", false);
            animator.SetInteger("View Direction", (int)viewDirection);
            animator.SetInteger("Role", (int)role);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // /*for debugging fieldOfView
        //  * will delete when testing is done
        // */
        // Vector3 mousePosition = Input.mousePosition;
        // Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        // Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;
        // float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
        // float angleToMouse = radiansToMouse * 180f / Mathf.PI;
        // fieldOfView.setDirection(angleToMouse);
        // fieldOfView.setOrigin(transform.position);

        if (animator)
        {
            animator.SetBool("isMoving", false);
            animator.SetInteger("Role", (int)role);
            animator.SetInteger("View Direction", (int)viewDirection);
        }

        //directional movement with a-s-d-w
        Vector3 deltaVect = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 tempVect = new Vector3(-1, 0, 0);
            deltaVect += tempVect.normalized * SPEED * Time.deltaTime;

            viewDirection = ViewDirection.Left;
            if (animator)
            {
                animator.SetBool("isMoving", true);
                animator.SetInteger("View Direction", (int)viewDirection);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 tempVect = new Vector3(1, 0, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            deltaVect += tempVect.normalized * SPEED * Time.deltaTime;

            viewDirection = ViewDirection.Right;
            if (animator)
            {
                animator.SetBool("isMoving", true);
                animator.SetInteger("View Direction", (int)viewDirection);
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 tempVect = new Vector3(0, 1, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            deltaVect += tempVect.normalized* SPEED *Time.deltaTime;

            viewDirection = ViewDirection.Up;
            if (animator)
            {
                animator.SetBool("isMoving", true);
                animator.SetInteger("View Direction", (int)viewDirection);
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 tempVect = new Vector3(0, -1, 0);
            tempVect = tempVect.normalized * SPEED * Time.deltaTime;
            deltaVect += tempVect.normalized* SPEED *Time.deltaTime;

            viewDirection = ViewDirection.Down;
            if (animator)
            {
                animator.SetBool("isMoving", true);
                animator.SetInteger("View Direction", (int)viewDirection);
            }
        }
        rb.MovePosition(transform.position + deltaVect);
        if (npcsInRange.Count != 0)
        {
            findClosestRole();
        }
        else {
            closestRole = Role.Player;
        }
        
        //change player sprites to fit colors
        //can take out some colors or add refresh time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //sr.sprite = getSprite(closestRole);
            if (npcsInRange.Count != 0 && closestRole != role)
            {
                updateRoleAndSprite();
                canChangeRole = false;
                ChangeRoleEvent.Invoke();
            }
        }

        if (GameComplete)
        {
            viewDirection = ViewDirection.Down;
            animator.SetBool("isMoving", false);
            animator.SetInteger("View Direction", (int)viewDirection);
            transform.position = Vector3.zero;
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     //collision with another NPC
    //     //check color to see if player reset
    //     if (collision.gameObject.GetComponent<NPC>())
    //     {
    //         //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //     }

    // }

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
            //disable the glow if it is the closest one in range
            otherNPC.transform.Find("Glow").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            npcsInRange.Remove(otherNPC);
        }
    }
    //do necessary changes when role is switched
    private void updateRoleAndSprite()
    {
        role = closestRole;
        sr.sprite = getSprite(role);

        if (animator)
        {
            animator.SetInteger("Role", (int)role);
            
        }
        // GetComponent<CapsuleCollider2D>().size = sr.sprite.bounds.size;
    }
    
    private void findClosestRole()
    {
        Vector3 playerPosition = transform.position;
        Vector3 closest = playerPosition;
        float closestDistance = 1000f;
        GameObject closestNPC = null;
        foreach(GameObject npc in npcsInRange)
        {
            Vector3 npcPosition = npc.transform.position;
            float npcDistance = Vector3.Distance(npcPosition, playerPosition);
            if(npcDistance <= closestDistance)
            {
                closestDistance = npcDistance;
                closest = npcPosition;
                closestRole = npc.GetComponent<NPC>().role;
                closestNPC = npc;
            }
            //disable glow for each
            switchNPCGlow(npc, false);
        }
        //set a glow on the closest NPC
        switchNPCGlow(closestNPC, true);
    }

    private void switchNPCGlow(GameObject NPCgo, bool ifOn)
    {
        if (NPCgo != null)
        {
            GameObject closestNPCGlow = NPCgo.transform.Find("Glow").gameObject;
            if (closestNPCGlow != null)
            {
                closestNPCGlow.transform.GetComponent<SpriteRenderer>().enabled = ifOn;
            }
        }
    }
    public Sprite getSprite(Role role)
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
            case Role.Noble:
                return nobleSprite;
            default:
                return starterSprite;
        }
    }

    // ShapeshiftTimerController: UnityEvent CooldownCompleteEvent
    public void onCooldownComplete() {
        // Debug.Log("onCooldownComplete");
        canChangeRole = true;
    }

    // ShapeshiftTimerController: UnityEvent TimerCompleteEvent
    public void onTimerComplete() {
        // Debug.Log("onTimerComplete");
        role = Role.Player;
        sr.sprite = getSprite(role);
    }

    public Role getCurrentRole() {
        return role;
    }
    
    public Role getClosestRole() {
        return closestRole;
    }
}
