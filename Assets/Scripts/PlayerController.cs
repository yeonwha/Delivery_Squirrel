using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] Animator anim;

    private float horizInput;           // store horizontal input for used in FixedUpdate()
    private float moveSpeed = 450.0f;   // 4.5 * 100 newtons

    private float jumpHeight = 3.0f;    // height of jump in units
    private float jumpTime = 0.75f;     // time of jump in seconds
    private float initialJumpVelocity;  // calculated jump velocity

    private bool isGrounded = false;    // true if player is grounded
    [SerializeField] private Transform groundCheckPoint;    // draw a circle around this to check ground
    [SerializeField] private LayerMask groundLayerMask;     // a layer for all things ground
    private float groundCheckRadius = 0.3f;                 // radius of ground check circle

    private int jumpMax = 2;            // # of jumps player can do without touching ground
    private int jumpsAvailable = 0;     // current jumps available to player

    private bool facingRight = true;    // true if facing right

    private List<FoodItem> foods;
    private string[] enemies = { "Opossum", "Pig", "Vulture" };

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip opossumSound;
    [SerializeField] private AudioClip vultureSound;
    [SerializeField] private AudioClip pigSound;

    private AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        // calculate gravity using gravity formula
        float timeToApex = jumpTime / 2.0f;
        float gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        Debug.Log("gravity:" + gravity);

        // adjust gravity scale of player based on gravity required for jumpHeight & jumpTime
        rbody.gravityScale = gravity / Physics2D.gravity.y;

        // calculate jump velocity req'd for jumpHeight & jumpTime
        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);

        
    }

    // Update is called once per frame
    void Update()
    {
        // read & store horizontal input
        horizInput = Input.GetAxis("Horizontal");

        // determine if player is running (for animator param)
        bool isRunning = horizInput > 0.01 || horizInput < -0.01;
        anim.SetBool("isRunning", isRunning);   // notify animator

        // determine if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) && rbody.velocity.y < 0.01;
        anim.SetBool("isGrounded", isGrounded); // notify animator

        // reset jumps if grounded
        if (isGrounded)
        {
            jumpsAvailable = jumpMax;
        }

        // if jump is triggered & available - go for it
        if (Input.GetButtonDown("Jump") && jumpsAvailable > 0)
        {
            Jump();
        }

        // Flip player if appropriate
        if ((facingRight && horizInput < -0.01) ||
            (!facingRight && horizInput > 0.01))
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        // draw the ground check sphere in the Scene
        Gizmos.DrawSphere(groundCheckPoint.position, groundCheckRadius);
    }

    private void FixedUpdate()
    {
        // move the player (use horizontal input, but maintain existing y velocity)
        float xVel = horizInput * moveSpeed * Time.deltaTime;
        rbody.velocity = new Vector2(xVel, rbody.velocity.y);
    }

    void Jump()
    {
        //Debug.Log("Jump!");
        // tell the player to jump
        rbody.velocity = new Vector2(rbody.velocity.x, initialJumpVelocity);
        jumpsAvailable--;
        anim.SetTrigger("jump");    // notify animator

        audioSrc.PlayOneShot(jumpSound);  // jump sound effect play one time
    }

    void Flip()
    {
        // flip the direction the player is facing
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // when contacting enemies
        if (enemies.Contains(collision.gameObject.tag))
        {
            switch (collision.gameObject.tag)
            {
                case "Opossum":
                    audioSrc.PlayOneShot(opossumSound); 
                    break;
                case "Vulture":
                    audioSrc.PlayOneShot(vultureSound);
                    break;
                case "Pig":
                    audioSrc.PlayOneShot(pigSound);
                    break;
            }
            anim.SetTrigger("hurt");
            Messenger<string>.Broadcast(GameEvent.ENEMY_CONTACT, collision.gameObject.tag);
        }
    }

    public void addFoods(FoodItem food)
    {
        foods.Add(food);
    }

    public List<FoodItem> getFoods()
    {
        return foods;
    }

    public void Die()
    {
        
    }

    public void Falling()
    {
        anim.SetTrigger("hurt");
    }

    public void Respawn(Transform startPt)
    {
        rbody.velocity = Vector3.zero;
        transform.position = startPt.position;
        Physics.SyncTransforms();
    }
}
