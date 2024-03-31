using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] Animator anim;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] GameObject player;

    private float horizInput;

    private bool facingRight = true;    // true if facing right
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizInput = transform.position.x;

        // Flip player if appropriate
        if ((facingRight && horizInput < -0.01) ||
            (!facingRight && horizInput > 0.01))
        {
            Flip();
        }
    }

    void Flip()
    {
        // flip the direction the player is facing
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // for (int i = 0; i < player.getFoods().;
    }
}
