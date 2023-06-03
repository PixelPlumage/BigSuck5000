using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float jumpSpeed = 5;
	[SerializeField] private LayerMask jumpableGround;
    

	private float dirX;
    private bool isJumping = false;
    
    enum MovementState
    {
        Idle,
        Moving,
        Jumping,
        Landing
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && !anim.GetBool("isSucc"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            isJumping = true;
        }
        if (Input.GetMouseButtonDown((int)MouseButton.Left) && isGrounded() && !anim.GetBool("isSucc"))
        {
            anim.SetBool("isAttacking", true);
        }
        if (Input.GetKeyDown(KeyCode.F) && isGrounded())
        {
            anim.SetBool("isSucc", true);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetBool("isSucc", false);
        }
        updatePlayerState();
    }

    private void resetAttack()
    {
		anim.SetBool("isAttacking", false);
	}

    private void updatePlayerState()
    {
        MovementState state;
        if (dirX > 0)
        {
            sprite.flipX = false;
            state = MovementState.Moving;
        } 
        else if (dirX < 0)
        {
            sprite.flipX = true;
            state = MovementState.Moving;
		}
        else
        {
            state = MovementState.Idle;
        }
        if (rb.velocity.y > 0.1f)
        {
			state = MovementState.Jumping;
		} else if (isJumping && isGrounded())
        {
            state = MovementState.Landing;
            isJumping = false;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.1f, jumpableGround);
    }
}
